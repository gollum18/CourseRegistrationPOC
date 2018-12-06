/**
 * Validates that an abbreviation was entered and that it is valid.
 * @param {string} abbreviation A value from an abbreviation textbox.
 */
function validateAbbreviation(abbreviation) {
    if (isEmptyOrWhiteSpace(abbreviation)) {
        return { result: false, error: "Error: You must enter an abbreviation!" };
    }
    if (!isNaN(abbreviation)) {
        return { result: false, error: "Error: You must enter a string for the abbreviation!" };
    }
    if (abbreviation.length > 4) {
        return { result: false, error: "Error: Abbreviation cannot be greater than 4 characters in length!" };
    }
    return { result: true, error: "" };
}

/**
 * Validates that a name was entered and that it is valid.
 * @param {string} name A value from a name textbox.
 */
function validateName(name) {
    if (isEmptyOrWhiteSpace(name)) {
        return {result:false, error:"Error: You must enter a name!"};
    }
    if (!isNaN(name)) {
        return {result:false, error:"Error: You must a string for the name!"};
    }
    if (name.length < 8 || name.length > 64) {
        return {result:false, error:"Error: Name must be [8, 64] characters in length!"};
    }
    return { result: true, error: "" };
}

/**
 * Validates that a selection was made in a dropdown.
 * @param {string} selection A value from a dropdown.
 * @param {string} item The item the dropdown represents.
 */
function validateSelection(selection, item) {
    if (isEmptyOrWhiteSpace(selection)) {
        return { result: false, error: "Error: You must select a " + item + "!" };
    }
    if (selection === "-1") {
        return { result: false, error: "Error: You must make a selection from the " + item + "dropdown!" };
    }
    if (isNaN(selection)) {
        return { result: false, error: "Error You have selected an invalid " + item + "!" };
    }
    return { result: true, error: "" };
}
