namespace HRMS.Application.Dtos;

public class TranslatorMessageDto(string text, string[] args)
{
    public string Text { get; set; } = text;
    public string[] Args { get; set; } = args;
}