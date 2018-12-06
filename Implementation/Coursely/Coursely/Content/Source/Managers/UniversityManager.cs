using System;
using System.Linq;
using System.Collections.Generic;

using Coursely.Content.Cache;
using Coursely.Content.Classes;

namespace Coursely.Content.Managers
{
    /// <summary>
    /// Manages instances of the Building, Department, Major, and School classes. This includes adding, retrieving, and updating 
    /// instances of these classes.
    /// </summary>
    public class UniversityManager
    {
        //
        // ATTRIBUTES
        //

        /// <summary>
        /// Caches instances of the Department class.
        /// </summary>
        private MemoryCache<int, Department> DepartmentCache;
        /// <summary>
        /// Caches instances of the School class.
        /// </summary>
        private MemoryCache<int, School> SchoolCache;
        /// <summary>
        /// Caches instances of the Major class.
        /// </summary>
        private MemoryCache<int, Major> MajorCache;
        /// <summary>
        /// Caches instances of the Building class.
        /// </summary>
        private MemoryCache<int, Building> BuildingCache;

        /// <summary>
        /// A singleton of the UniversityManager class.
        /// </summary>
        private static UniversityManager Instance { get; set; }

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Private constuctor used by the singleton design model.
        /// </summary>
        private UniversityManager()
        {
            BuildingCache = new MemoryCache<int, Building>();
            DepartmentCache = new MemoryCache<int, Department>();
            MajorCache = new MemoryCache<int, Major>();
            SchoolCache = new MemoryCache<int, School>();
        }

        /// <summary>
        /// Returns a pointer to a single instance of a university manager.
        /// </summary>
        /// <returns>An instance of a UniversityManager.</returns>
        public static UniversityManager InstanceOf()
        {
            if (Instance == null) {
                Instance = new UniversityManager();
            }
            return Instance;
        }

        //
        // METHODS
        //

        /// <summary>
        /// Logical layer method for adding a building.
        /// </summary>
        /// <param name="name">The building name.</param>
        /// <param name="abbreviation">The abbreviation of the building name.</param>
        /// <returns>True if the building was successfully added, false otherwise.</returns>
        public bool AddBuilding(string name, string abbreviation)
        {
            try
            {
                if (DoesBuildingExist(name))
                {
                    throw new Exception($"Error: A building already exists with the name: {name}!");
                }
                return DatabaseManager.InstanceOf().AddBuilding(name, abbreviation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logical layer method for adding a department.
        /// </summary>
        /// <param name="schoolID">The school identifier.</param>
        /// <param name="name">The department name.</param>
        /// <param name="abbreviation">The department identifier.</param>
        /// <returns>Treu if the department was successfully added, false otherwise.</returns>
        public bool AddDepartment(int schoolID, string name, string abbreviation)
        {
            try
            {
                if (!DoesSchoolExist(schoolID))
                {
                    throw new Exception($"Error: The indicated school does not exist!");
                }
                if (DoesDepartmentExist(schoolID, name))
                {
                    throw new Exception($"Error: A department already exists with the name: {name} in the " +
                        "indicated school!");
                }
                return DatabaseManager.InstanceOf().AddDepartment(schoolID, name, abbreviation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logical layer method for adding a major.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <param name="name">The name of the major.</param>
        /// <returns>True if the major was successfully added, false otherwise.</returns>
        public bool AddMajor(int departmentID, string name)
        {
            try
            {
                if (DoesMajorExist(departmentID, name))
                {
                    throw new Exception($"Error: A major with the name {name} already exists in the " +
                        $"indicated department!");
                }
                return DatabaseManager.InstanceOf().AddMajor(departmentID, name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logical layer method for adding a school.
        /// </summary>
        /// <param name="name">The school name.</param>
        /// <param name="abbreviation">The abbreviation of the school name.</param>
        /// <returns>True if the school was succssfully added, false otherwise.</returns>
        public bool AddSchool(string name, string abbreviation)
        {
            try
            {
                if (DoesSchoolExist(name))
                {
                    throw new Exception($"Error: A school already exists with the name: {name}!");
                }
                return DatabaseManager.InstanceOf().AddSchool(name, abbreviation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a specified building exists using a building identifier.
        /// </summary>
        /// <param name="buildingID">A building identifier.</param>
        /// <returns>True if a building exists, false otherwise.</returns>
        public bool DoesBuildingExist(int buildingID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesBuildingExist(buildingID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a building exists with a given name.
        /// </summary>
        /// <param name="name">A building name.</param>
        /// <returns>True if a building exists, false otherwise.</returns>
        public bool DoesBuildingExist(string name)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesBuildingExist(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a department exists with the given department identifier.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(int departmentID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesDepartmentExist(departmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if a department exists with the given department identifier and name.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="name">A department name.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(int departmentID, string name)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesDepartmentExist(departmentID, name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a department exists with the given abbreviation.
        /// </summary>
        /// <param name="abbreviation">An abbreviation of a department name.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesDepartmentExist(string abbreviation)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesDepartmentExist(abbreviation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a given major exists or not based on its major identifier.
        /// </summary>
        /// <param name="majorID">A major identifier.</param>
        /// <returns>True if a major exists with the given major identifier, false otherwise.</returns>
        public bool DoesMajorExist(int majorID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesMajorExist(majorID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a major exists within a department with the given name.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <param name="name">A deparment name.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public bool DoesMajorExist(int departmentID, string name)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesMajorExist(departmentID, name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a school exists with the given identifier.
        /// </summary>
        /// <param name="schoolID">A school identifier.</param>
        /// <returns>True if a school exists, false otherwise.</returns>
        public bool DoesSchoolExist(int schoolID)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesSchoolExist(schoolID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether a school with a specific name exists.
        /// </summary>
        /// <param name="name">A school name.</param>
        /// <returns>True if a school exists, false otherwise.</returns>
        public bool DoesSchoolExist(string name)
        {
            try
            {
                return DatabaseManager.InstanceOf().DoesSchoolExist(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a building from the database with the given building identifier.
        /// </summary>
        /// <param name="buildingID">A building identifier.</param>
        /// <returns>A building if it exists, false otherwise.</returns>
        public Building GetBuilding(int buildingID)
        {
            try
            {
                if (BuildingCache.Contains(buildingID))
                {
                    return BuildingCache.Get(buildingID);
                }
                Building building = DatabaseManager.InstanceOf().GetBuilding(buildingID);
                BuildingCache.Add(buildingID, building);
                return building;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a building identifier based on the buildings name.
        /// </summary>
        /// <param name="name">The name of the building.</param>
        /// <returns>A building identifier if the building exists.</returns>
        public int GetBuildingID(string name)
        {
            try
            {
                int buildingID = DatabaseManager.InstanceOf().GetBuildingID(name);
                if (buildingID == DatabaseManager.NOT_FOUND)
                {
                    throw new Exception($"Error: No building found that goes by {name}!");
                }
                return buildingID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of buildings from the database.
        /// </summary>
        /// <returns>A list of buildings.</returns>
        public List<Building> GetBuildings()
        {
            try
            {
                List<Building> buildings = DatabaseManager.InstanceOf().GetBuildings();
                buildings.AddRange(BuildingCache.GetValues());
                foreach (Building building in buildings)
                {
                    if (!BuildingCache.Contains(building.BuildingID)) {
                        BuildingCache.Add(building.BuildingID, building);
                    }
                }
                buildings.Sort();
                return buildings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a department using the specified department identifier.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>A department there is one that exists with the given identifier.</returns>
        public Department GetDepartment(int departmentID)
        {
            try
            {
                if (DepartmentCache.Contains(departmentID))
                {
                    return DepartmentCache.Get(departmentID);
                }
                Department department = DatabaseManager.InstanceOf().GetDepartment(departmentID);
                DepartmentCache.Add(departmentID, department);
                return department;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a department is for a given department.
        /// </summary>
        /// <param name="abbreviation">A department identifier.</param>
        /// <returns>True if a department exists, false otherwise.</returns>
        public int GetDepartmentID(string abbreviation)
        {
            try
            {
                return DatabaseManager.InstanceOf().GetDepartmentID(abbreviation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the list of departments from cache and the database that belong to the indicated school.
        /// </summary>
        /// <param name="schoolID">The school identifier.</param>
        /// <returns>A list of departments.</returns>
        /// <exception cref="Exception">If there was an issue connecting to the database.</exception>
        public List<Department> GetDepartments(int schoolID)
        {
            try
            {
                List<Department> departments = DatabaseManager.InstanceOf().GetDepartments(schoolID);
                departments.AddRange(DepartmentCache.GetValues().Where(d => d.SchoolID == schoolID));
                foreach (Department department in departments)
                {
                    if (!DepartmentCache.Contains(department.DepartmentID))
                    {
                        DepartmentCache.Add(department.DepartmentID, department);
                    }
                }
                departments.Sort();
                return departments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a major using the specified major identifier.
        /// </summary>
        /// <param name="majorID">A major identifier.</param>
        /// <returns>A major if one exists with the specified major identifier.</returns>
        public Major GetMajor(int majorID)
        {
            try
            {
                if (MajorCache.Contains(majorID))
                {
                    return MajorCache.Get(majorID);
                }
                Major major = DatabaseManager.InstanceOf().GetMajor(majorID);
                MajorCache.Add(majorID, major);
                return major;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a major identifier from the database using the specified major name.
        /// </summary>
        /// <param name="name">A major name.</param>
        /// <returns>A major identifier if the major exists.</returns>
        public int GetMajorID(string name)
        {
            try
            {
                int majorID = DatabaseManager.InstanceOf().GetMajorID(name);
                if (majorID == DatabaseManager.NOT_FOUND)
                {
                    throw new Exception($"Error: No major found that goes by {name}!");
                }
                return majorID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of majors from cache and the database with that belong to the indicated department.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns>A list of majors.</returns>
        public List<Major> GetMajors(int departmentID)
        {
            try
            {
                List<Major> majors = DatabaseManager.InstanceOf().GetMajors(departmentID);
                majors.AddRange(MajorCache.GetValues().Where(major => major.DepartmentID == departmentID));
                foreach (var major in majors)
                {
                    if (!MajorCache.Contains(major.MajorID))
                    {
                        MajorCache.Add(major.MajorID, major);
                    }
                }
                majors.Sort();
                return majors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a school from cache or the database.
        /// </summary>
        /// <param name="schoolID">A school identifier.</param>
        /// <returns>A school if it exists.</returns>
        public School GetSchool(int schoolID)
        {
            try
            {
                if (SchoolCache.Contains(schoolID))
                {
                    return SchoolCache.Get(schoolID);
                }
                School school = DatabaseManager.InstanceOf().GetSchool(schoolID);
                SchoolCache.Add(schoolID, school);
                return school;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a school identifier from the database using the school name.
        /// </summary>
        /// <param name="name">A school name.</param>
        /// <returns>A school identifier if the school exists, NOT_FOUND otherwise.</returns>
        public int GetSchoolID(string name)
        {
            try
            {
                int schoolID = DatabaseManager.InstanceOf().GetSchoolID(name);
                if (schoolID == DatabaseManager.NOT_FOUND)
                {
                    throw new Exception($"Error: No school found that goes by {name}!");
                }
                return schoolID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the list of schools from cache and the database.
        /// </summary>
        /// <returns>All of the schools in the database.</returns>
        /// <exception cref="Exception">If there was an issue connecting to the database.</exception>
        public List<School> GetSchools()
        {
            try
            {
                List<School> schools = DatabaseManager.InstanceOf().GetSchools();
                schools.AddRange(SchoolCache.GetValues());
                foreach (var school in schools)
                {
                    if (!SchoolCache.Contains(school.SchoolID))
                    {
                        SchoolCache.Add(school.SchoolID, school);
                    }
                }
                schools.Sort();
                return schools;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines if there is a building in cache with the specified identifier.
        /// </summary>
        /// <param name="buildingID">A building identifier.</param>
        /// <returns>True if the building is in cache, false otherwise.</returns>
        public bool HasBuildingCached(int buildingID)
        {
            return BuildingCache.Contains(buildingID);
        }

        /// <summary>
        /// Determines if there is a department in cache with the specified identifier.
        /// </summary>
        /// <param name="departmentID">A department identifier.</param>
        /// <returns>True if the department is in cache, false otherwise.</returns>
        public bool HasDepartmentCached(int departmentID)
        {
            return DepartmentCache.Contains(departmentID);
        }

        /// <summary>
        /// Determines if there is a major in cache with the specified identifier.
        /// </summary>
        /// <param name="majorID">A major identifier.</param>
        /// <returns>True if the major is in cache, false otherwise.</returns>
        public bool HasMajorCached(int majorID)
        {
            return MajorCache.Contains(majorID);
        }

        /// <summary>
        /// Determines if there is a school in cache with the specified identifier.
        /// </summary>
        /// <param name="schoolID">A school identifier.</param>
        /// <returns>True if the school is in cache, false otherwise.</returns>
        public bool HasSchoolCached(int schoolID)
        {
            return SchoolCache.Contains(schoolID);
        }

        /// <summary>
        /// Modifies a building with the specified parameters.
        /// </summary>
        /// <param name="buildingID">The building's identifier.</param>
        /// <param name="abbreviation">The new abbreviation.</param>
        /// <returns>True if the building was successfully modified, false otherwise.</returns>
        public bool ModifyBuilding(int buildingID, string name, string abbreviation)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifyBuilding(buildingID, name, abbreviation) &&
                        BuildingCache.Contains(buildingID))
                {
                    BuildingCache.Get(buildingID).Abbreviation = abbreviation;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies a department with the specified parameters.
        /// </summary>
        /// <param name="departmentID">The department's identifier.</param>
        /// <param name="schoolID">The identifier for the school the department belongs to.</param>
        /// <param name="abbreviation">The department's abbreviation.</param>
        /// <returns>True if the department was successfully modified, false otherwise.</returns>
        public bool ModifyDepartment(int departmentID, int schoolID, string abbreviation)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifyDepartment(departmentID, schoolID, abbreviation) &&
                        DepartmentCache.Contains(departmentID))
                {
                    Department temp = DepartmentCache.Get(departmentID);
                    temp.SchoolID = schoolID;
                    temp.Abbreviation = abbreviation;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies a major with the specified parameters.
        /// </summary>
        /// <param name="majorID">The major's identifier.</param>
        /// <param name="departmentID">The identifier of the department the major belongs to.</param>
        /// <returns>True if the major was successfully modified, false otherwise.</returns>
        public bool ModifyMajor(int majorID, int departmentID)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifyMajor(majorID, departmentID) && 
                        MajorCache.Contains(majorID))
                {
                    MajorCache.Get(majorID).DepartmentID = departmentID;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Modifies a school with the specified parameters.
        /// </summary>
        /// <param name="schoolID">The school's identifier.</param>
        /// <param name="abbreviation">The abbreviation of the school's name.</param>
        /// <returns>True if the school was successfully modified, false otherwise.</returns>
        public bool ModifySchool(int schoolID, string abbreviation)
        {
            try
            {
                if (DatabaseManager.InstanceOf().ModifySchool(schoolID, abbreviation) && 
                        SchoolCache.Contains(schoolID))
                {
                    SchoolCache.Get(schoolID).Abbreviation = abbreviation;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}