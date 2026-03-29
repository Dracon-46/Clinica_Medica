// Domain/Interfaces/IReceituario.cs
namespace ClinicaMVC.Domain.Interfaces;

public interface IReceituario
{
    void Emitir();
    string GetPrescricao();
}
