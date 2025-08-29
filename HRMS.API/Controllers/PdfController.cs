using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController(IHttpClientFactory httpClientFactory): ControllerBase
{
    /// <summary>
    /// Merge uploaded PDF files (multipart/form-data).
    /// </summary>
    /// <remarks>
    /// Upload at least two PDFs. The API returns a single merged PDF.
    /// </remarks>
    /// <param name="form">Form containing PDF files.</param>
    /// <returns>Merged PDF as a file response.</returns>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MergeUploadedAsync([FromForm] MergeFilesRequest form)
    {
        if (form?.Files == null || form.Files.Count < 2)
            return BadRequest("Please upload at least two files (PDFs and/or images) as 'Files'.");

        // Load all files to memory safely (PdfSharpCore image loader may reopen streams)
        var fileBlobs = new List<(string Name, string? ContentType, byte[] Bytes)>();
        foreach (var f in form.Files)
        {
            if (f.Length <= 0) continue;
            await using var ms = new MemoryStream();
            await f.CopyToAsync(ms);
            fileBlobs.Add((f.FileName, f.ContentType, ms.ToArray()));
        }
        
        if (fileBlobs.Count < 2)
            return BadRequest("Please upload at least two non-empty files.");

        using var outputDoc = new PdfDocument();

        foreach (var (name, contentType, bytes) in fileBlobs)
        {
            try
            {
                if (IsPdf(name, contentType, bytes))
                {
                    using var src = PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Import);
                    for (int i = 0; i < src.PageCount; i++)
                        outputDoc.AddPage(src.Pages[i]);
                }
                else if (IsSupportedImage(name, contentType, bytes))
                {
                    AddImageAsPdfPage(outputDoc, bytes);
                }
                else
                {
                    return BadRequest($"Unsupported file type for '{name}'. Upload PDFs or images (jpg, jpeg, png, bmp, gif, tiff).");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to process '{name}': {ex.Message}");
            }
        }

        await using var outStream = new MemoryStream();
        outputDoc.Save(outStream, false);
        outStream.Position = 0;
        return File(outStream.ToArray(), MediaTypeNames.Application.Pdf, "merged.pdf");

        
    }
    
    /// <summary>
    /// Multipart/form-data request for merging PDFs.
    /// </summary>
    public class MergeFilesRequest
    {
        /// <summary>
        /// PDF files to merge (at least 2).
        /// </summary>
        [Required]
        public List<IFormFile> Files { get; set; } = new();
    }
    private static bool IsPdf(string fileName, string? contentType, byte[] bytes)
    {
        if (!string.IsNullOrWhiteSpace(contentType) &&
            contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return true;

        if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            return true;

        // Magic header: %PDF-
        return bytes.Length >= 5 &&
               bytes[0] == 0x25 && bytes[1] == 0x50 &&
               bytes[2] == 0x44 && bytes[3] == 0x46 && bytes[4] == 0x2D;
    }

    private static readonly string[] ImageExts = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif", ".tiff" };

    private static bool IsSupportedImage(string fileName, string? contentType, byte[] bytes)
    {
        if (!string.IsNullOrWhiteSpace(contentType) && contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return true;

        if (ImageExts.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            return true;

        // Very light-weight magic checks for common types (best-effort; we rely mostly on Content-Type/extension)
        // JPEG FF D8, PNG 89 50 4E 47, GIF "GIF8", BMP "BM", TIFF "II*^@", "MM^@*"
        if (bytes.Length > 4)
        {
            // JPEG
            if (bytes[0] == 0xFF && bytes[1] == 0xD8) return true;
            // PNG
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) return true;
            // GIF
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38) return true;
            // BMP
            if (bytes[0] == 0x42 && bytes[1] == 0x4D) return true;
            // TIFF (II*\0 or MM\0*)
            if ((bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00) ||
                (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A))
                return true;
        }

        return false;
    }

    private static void AddImageAsPdfPage(PdfDocument outputDoc, byte[] imageBytes)
    {
        // PdfSharpCore recommends providing a stream factory, since the image decoder may need to re-read.
        using var img = XImage.FromStream(() => new MemoryStream(imageBytes));

        // Create a new page sized to the image's dimensions in points (honors DPI metadata).
        var page = outputDoc.AddPage();
        page.Width = img.PointWidth;
        page.Height = img.PointHeight;

        using var gfx = XGraphics.FromPdfPage(page);
        // Draw to full page (keeps aspect ratio because page matches image size)
        gfx.DrawImage(img, 0, 0, page.Width, page.Height);
    }
}