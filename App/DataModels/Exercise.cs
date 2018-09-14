// <copyright file="Exercise.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.DataModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

  /// <summary>
  /// Exercise object which is a container for section objects that compose the exercise.
  /// This object also holds a name for the exercise as well as a saved default start time for the exercise.
  /// </summary>
  [Table("Exercise", Schema = "dbo")]
  public class Exercise
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Exercise"/> class.
    /// Default constructor.
    /// </summary>
    public Exercise()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exercise"/> class.
    /// Helper constructor for transforming DTOs into database objects.
    /// </summary>
    /// <param name="exerciseDTO">A <see cref="Models.Section"/> DTO.</param>
    public Exercise(Models.Exercise exerciseDTO)
    {
      Id = exerciseDTO.Id;
      Name = exerciseDTO.Name;
      Created = exerciseDTO.Created != null ? exerciseDTO.Created : DateTime.Now;
      StartTime = exerciseDTO.StartTime;

      // when saving a new fully defined exercise we should also include the sections.
      Sections = exerciseDTO.Sections != null
      ? exerciseDTO.Sections.Select(s => new Section(s)).ToList()
      : new List<Section>();
    }

    /// <summary>
    /// Gets or sets the Id of the Exercise.
    /// </summary>
    /// <value>int property: database Id.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name of the Exercise.
    /// </summary>
    /// <value>string property: exercise name.</value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Created timestamp of the Exercise.
    /// </summary>
    /// <value>DateTime property: created timestamp.</value>
    [Required]
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the StartTime of the Exercise.
    /// </summary>
    /// <value>DateTime property: start time.</value>
    [InverseProperty("Exercise")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="Section"/> linked to the Exercise.
    /// </summary>
    /// <value>List of linked sections.</value>
    public ICollection<Section> Sections { get; set; }
  }
}