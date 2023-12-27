using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RamWebAPI.Models;
using RamWebAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RamWebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public partial class FileSaveDateController : Controller
    {
        [HttpGet("saveDates")]
        public async Task<IActionResult> GetFileDates()
        {
            // Путь к XML-файлу с ссылками на репозитории файлов
            string xmlFilePath = @"/app/AddinsData.xml";

            //Чтение данных из XML
            List<FileData> repositories = FileService.ReadRepositoriesFromXml(xmlFilePath);

            //Получение даты сохранения файлов
            List<FileData> allFileDates = new List<FileData>();
            //Внутри цикла обращаемся к каждому репозиторию
            foreach (var repository in repositories)
            {
                try
                {
                    //Создание HttpClient для выполнения запросов к серверу
                    using (HttpClient httpClient = new HttpClient())
                    {
                        // Вызов асинхронного метода для получения списка даты сохранения файла
                        DateTime fileDates = await FileService.GetLocalFileLastModifiedDateAsync(repository.FilePath);

                        // Добавление полученных дат в общий список
                        allFileDates.Add(
                            new FileData()
                            {
                                FileName = repository.FileName,
                                FilePath = repository.FilePath,
                                LocalFileFolder = repository.LocalFileFolder,
                                Date = fileDates,
                                ChangeInfo = repository.ChangeInfo
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Ошибка при обращении к репозиторию: {ex.Message}");
                }
            }
            return Ok(allFileDates);
        }

        [HttpGet("file")]
        public IActionResult DownloadFile([FromQuery] string fileName)
        {
            string filePath = Path.Combine($"/app/{fileName}/", fileName + ".dll"); ;
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName + "dll", out var mimeType))
                {
                    mimeType = "application/octet-stream";
                }
                return File(fileBytes, mimeType, fileName);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
