﻿// <copyright file="Exercise.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Runtime.Serialization;

  /// <summary>
  /// A DTO version of the <see cref="DataModels.Exercise"/> object.
  /// </summary>
  [DataContract(Name = "exercise")]
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
    /// Helper constructor used to transform <see cref="DataModels.Exercise"/> objects to DTOs.
    /// </summary>
    /// <param name="exerciseData">A <see cref="DataModels.Exercise"/> object.</param>
    public Exercise(DataModels.Exercise exerciseData)
    {
      Id = exerciseData.Id;
      Name = exerciseData.Name;
      Created = exerciseData.Created != null ? exerciseData.Created : DateTime.Now;
      StartTime = exerciseData.StartTime;
      Sections = exerciseData.Sections.Select(s => new Section(s)).ToList();
    }

    /// <summary>
    /// Gets or sets the Id of the Exercise.
    /// </summary>
    /// <value>int property: database Id.</value>
    [DataMember(Name = "id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name of the Exercise.
    /// </summary>
    /// <value>string property: exercise name.</value>
    [DataMember(Name = "name")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Length of {0} must be between {2} and {1} characters.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Created timestamp of the Exercise.
    /// </summary>
    /// <value>DateTime property: created timestamp.</value>
    [DataMember(Name = "created")]
    [DataType(DataType.DateTime)]
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the StartTime of the Exercise.
    /// </summary>
    /// <value>DateTime property: start time.</value>
    [DataMember(Name = "startTime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets a list of <see cref="Models.Section"/> objects linked to the Exercise.
    /// </summary>
    /// <value>List of linked sections.</value>
    [DataMember(Name = "sections")]
    public ICollection<Section> Sections { get; set; }
  }
}