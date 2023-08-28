﻿using System.ComponentModel.DataAnnotations;

namespace Webservices.Client.Web.Models.Catalog;

public class CourseCreateInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string UserId { get; set; }
    public string Picture { get; set; }
    public FeatureViewModel Feature { get; set; }
    [Display(Name = "Category")]
    [Required]
    public string CategoryId { get; set; }
    [Display(Name = "Course Photo")]
    public IFormFile PhotoFormFile { get; set; }
}