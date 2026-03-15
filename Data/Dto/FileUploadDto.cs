using Microsoft.AspNetCore.Mvc;

namespace WebApiNibu.Data.Dto;

public class FileUploadDto
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;
}

