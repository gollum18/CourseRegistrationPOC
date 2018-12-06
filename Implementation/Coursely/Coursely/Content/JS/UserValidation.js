/*
 * Validates an email address.
 */
function validateEmail(email) {
    if (isEmptyOrWhiteSpace(email)) {
        return { error: 'Error: You must enter an email address!', result: false }
    }

    if (email.length > 128) {
        return { error: 'Error: Email must be less than 128 characters!', result: false };
    }

    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return { error: 'Error: You must enter a valid email address!', result: false }
    }

    return { error: '', result: true };
}

/*
 * Validates a name.
 */
function validateName(nm) {
    if (isEmptyOrWhiteSpace(nm)) {
        return { error: 'Error: You did not enter a name!', result: false };
    }
    if (nm.length > 32) {
        return { error: 'Error: Name must be less than or equal to 32 characters in length!', result: false };
    }
    return { error: '', result: true };
}

/*
 * Validates a password pair.
 */
function validatePassword(currentPassword, newPassword, confirmPassword) {
    if (isEmptyOrWhiteSpace(currentPassword) || isEmptyOrWhiteSpace(currentPassword) || 
            isEmptyOrWhiteSpace(confirmPassword)) {
        return { error: 'Error: Password cannot be blank!', result: false };
    }

    if (newPassword.length < 8) {
        return { error: 'Error: Password must be at least 8 characters!', result: false };
    }
   
    if (newPassword !== confirmPassword) {
        return { error: 'Error: Passwords do not match!', result: false };
    }

    currentPassword = null;
    newPassword = null;
    confirmPassword = null;

    return { error: '', result: true }
}

/*
 * Validates a university identifier.
 */
function validateUID(uid) {
    if (isEmptyOrWhiteSpace(uid)) {
        return { error: 'Error: You must enter a university identifer!', result: false }
    }
    if (uid.length < 6 || uid.length > 9) {
        return { error: 'Error: University identifier must be between 6 and 9 characters long!', result: false };
    }
    return { error: '', result: true };
}