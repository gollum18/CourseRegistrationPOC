using System;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Represents a building on campus.
    /// </summary>
    public class Building : IComparable<Building>
    {
        /// <summary>
        /// The unique identifier of the building.
        /// </summary>
        public int BuildingID { get; }
        /// <summary>
        /// The name of the building.
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// The abbreviation of the building.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Creates an instance of a building with the given parameters.
        /// </summary>
        /// <param name="buildingID">The unique building identifier.</param>
        /// <param name="name">The name of the building.</param>
        public Building(int buildingID, string name)
        {
            BuildingID = buildingID;
            Name = name;
            Abbreviation = default(string);
        }

        /// <summary>
        /// Determines if an object is equal to this building.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>True if the object is not null, is a building, and has he same name as this building.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Building))
            {
                return false;
            }
            return (obj as Building).BuildingID == BuildingID; 
        }

        /// <summary>
        /// Returns the unique hash code for this building.
        /// </summary>
        /// <returns>The building identifier, as it is already guaranteed to be unique.</returns>
        public override int GetHashCode()
        {
            return BuildingID;
        }

        /// <summary>
        /// Compares this bulding to another.
        /// </summary>
        /// <param name="other">The other building to compare against.</param>
        /// <returns>The natural ordering of this buildings name to the other buildings name.</returns>
        public int CompareTo(Building other)
        {
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Modifies a building with the given parameters.
        /// </summary>
        /// <param name="name">The building name.</param>
        /// <param name="abbreviation">The building abbreviation.</param>
        public void Modify(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }
}