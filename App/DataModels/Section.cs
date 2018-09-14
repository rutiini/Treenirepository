// <copyright file="Section.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.DataModels
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  /// <summary>
  /// Database model of the Section object.
  /// </summary>
  [Table("Section", Schema = "dbo")]
  public class Section
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Section"/> class.
    /// </summary>
    public Section()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Section"/> class.
    /// Helper constructor used to transform <see cref="Models.Section"/> DTOs to database objects.
    /// </summary>
    /// <param name="sectionDTO">A DTO version of the object <see cref="Models.Section"/>.</param>
    public Section(Models.Section sectionDTO)
    {
      Id = sectionDTO.Id;
      Name = sectionDTO.Name;
      Duration = sectionDTO.Duration;
      SetupDuration = sectionDTO.SetupDuration;
      Color = sectionDTO.Color;
      ExerciseId = sectionDTO.ExerciseId;
      Description = sectionDTO.Description;
    }

    /// <summary>
    /// Gets or sets the Id property of the Section.
    /// </summary>
    /// <value>int property: (database) Id.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name property of the Section.
    /// </summary>
    /// <value>string property: Name.</value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Description property of the Section.
    /// </summary>
    /// <value>string property: Description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the Duration property of the Section.
    /// </summary>
    /// <value>int property: Duration.</value>
    [Required]
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the SetupDuration property of the Section.
    /// Setupduration is used to indicate inactive time between sections.
    /// </summary>
    /// <value>int property: SetupDuration.</value>
    [Required]
    public int SetupDuration { get; set; }

    /// <summary>
    /// Gets or sets the Color property of the Section.
    /// Color indicates which theme color the section should render in the UI.
    /// </summary>
    /// <value>int property: Color.</value>
    [Required]
    public int Color { get; set; }

    /// <summary>
    /// Gets or sets the ExerciseId property of the Section.
    /// This property links the section to an <see cref="Exercise"/> object.
    /// </summary>
    /// <value>int property: ExerciseId.</value>
    [Required]
    public int ExerciseId { get; set; }

    /// <summary>
    /// Gets or sets the Navigation property <see cref="Exercise"/>.
    /// </summary>
    /// <value>navigation property: Exercise.</value>
    [ForeignKey(nameof(ExerciseId))]
    public Exercise Exercise { get; set; }
  }
}