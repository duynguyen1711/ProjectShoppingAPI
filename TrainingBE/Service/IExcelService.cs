namespace TrainingBE.Service
{
    public interface IExcelService
    {
        byte[] ExportToExcel<T>(List<T> data);
    }
}
