var promises = {}
var _promiseId = "";
var callTimeout = null;

export function callMAUIJS(method, timeout, message) {
    if (timeout > 0) {
        callTimeout = setTimeout(timesup, timeout * 1000);
    }
    // object for storing references to our promise-objects
    return createPromise(method, message)
        .then(function (returnedString) { clearTimeout(callTimeout); console.log("S" + returnedString); return returnedString; }
            , function (returnedString) { clearTimeout(callTimeout); console.log("F" + returnedString); return "Error:" + returnedString; }
        );
}

function timesup() {
    promises[_promiseId].reject("call MAUI timeout");
    delete promises[promiseId];
}
// generates a unique id, not obligator a UUID
function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}

// this funciton is called by native methods
// @param promiseId - id of the promise stored in global variable promises
window.resolvePromise = function (promiseId, data, error) {
    if (error) {
        promises[promiseId].reject(error);

    } else {
        promises[promiseId].resolve(data);
    }
    // remove referenfe to stored promise
    delete promises[promiseId];
}

function createPromise(method, args) {
    var promise = new Promise(function (resolve, reject) {
        // we generate a unique id to reference the promise later
        // from native function
        var promiseId = generateUUID();
        _promiseId = promiseId;
        // save reference to promise in the global variable
        promises[promiseId] = { resolve, reject };

        try {
            // call native function
            const parms = { method: method, promiseId: promiseId, args: args };
            invokeMAUIAction("callMAUI:" + JSON.stringify(parms) );
            //var tempdebug = generateUUID();
        }
        catch (exception) {
            console.log(exception);
            promises[promiseId].reject(exception);
        }
    });
    return promise;
}
