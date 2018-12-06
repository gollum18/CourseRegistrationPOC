/*
 * Validates that a course was selected.
 */
function validateCourse(course) {
    if (course === "-1") {
        return {result:false, error:"Error: Cannot delete course, no course selected!"};
    }
    return {result:true, error:""};
}

/*
 * Validates a department was selected for the course. 
 */
function validateCourseDepartment(department) {
    if (department === "-1") {
        return { result: false, error: "Error: Cannot create/modify course, no department selected!" };
    }

    return { result: true, error: "" };
}

/*
 * Validates the course name.
 */
function validateCourseName(name) {
    if (name.length == 0) {
        return { result: false, error: "Error: Cannot create/modify course, you must enter a course name!" };
    }

    if (name.length < 8 || name.length > 64) {
        return { result: false, error: "Error: Cannot create/modify course, course name must be [8, 64] characters in length!" };
    }

    return { result: true, error: "" };
}

/*
 * Validates the course number.
 */
function validateCourseNumber(number) {
    if (number < 1) {
        return { result: false, error: "Error: Cannot create/modify course, course number must be greater than 0!" };
    }

    if (number > 9999) {
        return { result: false, error: "Error: Cannot create/modify cousre, course number must be less than 10,000!" };
    }

    return { result: true, error: "" };
}

/*
 * Validates the course credits.
 */
function validateCourseCredits(credits) {
    if (credits < 1 || credits > 5) {
        return { result: false, error: "Error: Cannot create/modify course, course credits must be [1, 5]!" };
    }

    return { result: true, error: "" };
}

/*
 * Performs basic validation to ensure the prerequisites string is in the proper format.
 */
function validateCoursePrereqs(prereqs) {
    if (isEmptyOrWhiteSpace(prereqs)) {
        return {result:true, error:""};
    }

    var upper = prereqs.toUpperCase();

    if (upper.endsWith("AND") || upper.endsWith("OR")) {
        return { result: false, error: "Error: Course prerequisites are not in the correct format!" };
    }

    var current = "";
    var split = upper.split(" ");

    if (split.length == 1) {
        return {result:false, error:"Error: Course prerequisites are not in the correct format!"};
    }

    for (var i = 0; i < split.length - 1; i++) {
        current = split[i];
        if (current === "AND" || current === "OR") {
            // Error if next is a number or we end with a department abbreviation
            if (!isNaN(split[i + 1]) || i + 2 >= split.length) {
                return { result: false, error: "Error: Course prerequisites are not in the correct format!" }
            }
        } else {
            // Check for NaN, we are at a department abbreviation
            if (isNaN(current)) {
                // Error if next is a string
                if (isNaN(split[i + 1])) {
                    return { result: false, error: "Error: Course prerequisites are not in the correct format!" };
                }
            } else {
                // Error if we are not at the end of the prerequisites or the next is not a combinator
                if (!(isEmptyOrWhiteSpace(split[i+1]) || split[i+1] === "AND" || split[i+1] === "OR")) {
                    return {result:false, error: "Error: Course prerequisites are not in the correct format!"};
                }
            }
        }
    }

    return { result: true, error: "" };
}

/*
 * Validates the course description.
 */
function validateCourseDescription(description) {
    if (description.length == 0) {
        return { result: false, error: "Error: Cannot create/modify course, you must enter a description!" };
    }

    if (description.length < 64 || description.length > 1024) {
        return { result: false, error: "Error: Cannot create/modify course, course description must be [64, 1024] characters in length!" };
    }

    return { result: true, error: "" };
}