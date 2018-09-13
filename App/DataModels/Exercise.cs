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
    /// Default constructor.
    /// </summary>
    public Exercise()
    {
    }

    /// <summary>
    /// Helper constructor for transforming DTOs into database objects.
    /// </summary>
    /// <param name="exerciseDTO">A <see cref="Models.Section"/> DTO</param>
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
    /// <value></value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name of the Exercise.
    /// </summary>
    /// <value></value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Created timestamp of the Exercise.
    /// </summary>
    /// <value></value>
    [Required]
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the StartTime of the Exercise.
    /// </summary>
    /// <value></value>
    [InverseProperty("Exercise")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="Section"/> linked to the Exercise.
    /// </summary>
    /// <value></value>
    public ICollection<Section> Sections { get; set; }
  }
}