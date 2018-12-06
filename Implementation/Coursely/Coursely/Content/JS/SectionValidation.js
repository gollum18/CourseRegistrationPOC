function validateSection(section) {
    if (section === "-1") {
        return {result:false, error:"Error: Cannot delete section, no section selected!"};
    }
    return {result:true, error:""};
}

/*
 * Validates that the user selected a course.
 */
function validateSectionCourse(courseID) {
    if (courseID === "-1") {
        return { result: false, error:"Error: Cannot create/modify section, no course was selected!"};
    }
    return {result:true, error:""};
}

/*
 * Validates that the user selected a building.
 */
function validateSectionBuilding(buildingID) {
    if (buildingID === "-1") {
        return { result: false, error:"Error: Cannot create/modify section, no building was selected!"};
    }

    return { result: true, error: "" };
}

/*
 * Validates that the section number is in the range of [1, 999].
 */
function validateSectionNumber(number) {
    if (number < 1 || number > 999) {
        return { result: false, error:"Error: Cannot create/modify section, section number must be in the range of [1, 999]!"};
    }
    
    return { result: true, error: "" };
}

/*
 * Validates that the room number is in the range of [1, +inf).
 */
function validateSectionRoom(room) {
    if (room < 1 || room > 999) {
        return { result: false, error:"Error: Cannot create/modify section, room number must bein the range of [0, 9999]!"};
    }

    return { result: true, error: "" };
}

/*
 * Verifies that the starting date is before the ending date.
 */
function validateSectionDates(start, end) {
    if (start >= end) {
        return { result: false, error:"Error: Cannot create/modify section, start date must be before end date!"};
    }

    return { result: true, error: "" };
}

/*
 * Validates that the starting time is before the ending time.
 */
function validateSectionTimes(start, end) {
    if (start >= end) {
        return { result: false, error:"Error: Cannot create/modify section, start time must be before end time!"};
    }

    return { result: true, error: "" };
}

/*
 * Validates that the user entered an enrollment in the range of [1, 200].
 */
function validateSectionMaxEnrollment(enrollment) {
    if (enrollment < 5 || enrollment > 200) {
        return { result: false, error:"Error: Cannot create/modify section, max enrollment must be in the range of [1, 200]!"};
    }

    return {result:true, error:""};
}