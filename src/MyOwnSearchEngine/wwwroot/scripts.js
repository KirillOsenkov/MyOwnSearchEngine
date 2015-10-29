function onPageLoad() {
    searchTimerID = -1;
    lastQuery = null;
    lastSearchString = inputBox.value;

    inputBox.focus();

    inputBox.onkeyup = function () {
        if (event && event.keyCode == 13) {
            onSearchChange();
        }
    };

    inputBox.oninput = function () {
        onSearchChange();
    };

    var query = document.location.search;
    if (query) {
        query = query.slice(1);
        if (query) {
            query = decodeURIComponent(query);
            inputBox.value = query;
            lastSearchString = query;
            search();
        }
    }
}

function onSearchChange() {
    if (inputBox.value == lastSearchString) {
        return;
    }

    lastSearchString = inputBox.value;

    if (inputBox.value.length > 0) {
        if (searchTimerID == -1) {
            searchTimerID = setTimeout(runSearch, 600);
        }
    } else {
        loadResults("");
    }
}

function runSearch() {
    searchTimerID = -1;
    if (typeof lastQuery === "object" && lastQuery !== null) {
        lastQuery.abort();
        lastQuery = null;
    }

    search();
}

function search() {
    var query = "api/answers/?query=" + encodeURIComponent(inputBox.value);

    lastQuery = getUrl(query, loadResults);
}

function getUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Accept", "text/html");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var data = xhr.responseText;
            if (typeof data === "string" && data.length > 0) {
                callback(data);
            }
        }
    };
    xhr.send();
    return xhr;
}

function loadResults(data) {
    var container = document.getElementById("outputDiv");
    if (container) {
        container.innerHTML = data;
        if (data) {
            updateUrl();
        }
    }
}

function updateUrl() {
    var query = inputBox.value;
    if (query) {
        query = "?" + encodeURIComponent(query);
        if (document.location.search !== query) {
            top.history.replaceState(null, top.document.title, query);
        }
    }
}