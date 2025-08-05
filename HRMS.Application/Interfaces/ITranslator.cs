using HRMS.Application.Dtos;

namespace HRMS.Application.Interfaces;

public interface ITranslator
{
    string this[string name] { get; }
    string GetString(string name);
    string GetString(TranslatorMessageDto input);
}