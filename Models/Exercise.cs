
namespace treenirepository.Models
{

  using System;
  using System.Runtime.Serialization;
  
  [DataContract(Name="exercise")]
  public class Exercise
  {
    public Exercise()
    {
    }
    public Exercise(DataModels.Exercise exerciseData)
    {
      Id = exerciseData.Id;
      Name = exerciseData.Name;
      Created = exerciseData.Created != null ? exerciseData.Created : DateTime.Now;
      ContentData = exerciseData.ContentData;

    }
    [DataMember(Name="id")]
    public int Id { get; set; }
    [DataMember(Name="name")]
    public string Name { get; set; }
    [DataMember(Name="created")]
    public DateTime Created { get; set; }
    [DataMember(Name="contentData")]
    public string ContentData { get; set; }
  }
}