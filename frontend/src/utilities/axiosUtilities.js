export function getErrorMessageFromAxiosError(error) {
    if (error.response?.data?.errorMessage) {
        return error.response.data.errorMessage;
    }

    if (error.response?.data?.status) {
        const status = error.response.data.status;
        if (status >= 400 && status < 500) {
            const prefix = "There was an error with the request";
            if (error.response.data.statusText) {
                return `${prefix}: ${error.response.data.statusText}.`;
            }
            else if (error.response.data.errors) {
                const errorObject = error.response.data.errors;
                return `${prefix}: ${errorObject[Object.keys(errorObject)[0]][0]}`;
            }

            return prefix;
        }

        return "The server ran into an error.";
    }

    if (error.response) {
        const status = error.response.status;

        if (status === 404) {
            return "The specified service was not found.";
        }

        return error.response.statusText;
    }

    if (error.request) {
        return "Did not receive a response from the server.";
    }

    return `There was an error sending the request: ${error.message}`;
}