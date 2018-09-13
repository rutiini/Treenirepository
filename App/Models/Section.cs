namespace Treenirepository.Models
{

  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Runtime.Serialization;

  /// <summary>
  /// A DTO version of the <see cref="DataModels.Section"/> object.
  /// </summary>
  [DataContract(Name = "section")]
  public class Section
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Section()
    {
    }

    /// <summary>
    /// Helper constructor used to transform <see cref="DataModels.Section"/> objects to DTOs.
    /// </summary>
    /// <param name="sectionData">A <see cref="DataModels.Section"/> object</param>
    public Section(DataModels.Section sectionData)
    {
      Id = sectionData.Id;
      Name = sectionData.Name;
      Duration = sectionData.Duration;
      SetupDuration = sectionData.SetupDuration;
      Color = sectionData.Color;
      ExerciseId = sectionData.ExerciseId;
      Description = sectionData.Description;

    }

    /// <summary>
    /// Gets or sets the Id of the Section.
    /// </summary>
    /// <value></value>
    [DataMember(Name = "id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name of the Section.
    /// </summary>
    /// <value></value>
    [DataMember(Name = "name")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Length of {0} must be between {2} and {1} characters.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Description of the Section.
    /// </summary>
    /// <value></value>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the Duration of the Section.
    /// </summary>
    /// <value></value>
    [DataMember(Name = "duration")]
    [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the SetupDuration of the Section.
    /// see <see cref="DataModels.Section.SetupDuration"/> for more details
    /// </summary>
    /// <value></value>
    [DataMember(Name = "setupDuration")]
    [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int SetupDuration { get; set; }

    /// <summary>
    /// Gets or sets the Color of the Section.
    /// see <see cref="DataModels.Section.Color"/> for more details.
    /// </summary>
    /// <value></value>
    [DataMember(Name = "color")]
    [Range(0, 50, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Color { get; set; }

    /// <summary>
    /// Gets or sets the ExerciseId for the section. This propery links the section to an exercise.
    /// TODO: weigh whether we should even expose this property to users, we only use sections under an exercise for now..
    /// </summary>
    /// <value></value>
    [DataMember(Name = "exerciseId")]
    [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ExerciseId { get; set; }
  }
}