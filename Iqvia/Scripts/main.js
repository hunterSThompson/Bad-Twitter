var downloadCsv = function (tweets) {

    // Convert to Csv.js format.
    var lst = [];
    for (i = 0; i < tweets.length; i++) {
        var tweet = tweets[i];
        lst.push([tweet.stamp, tweet.text, tweet.id]);
    }

    // Generate properly escaped CSV string
    var csv = new CSV(lst);
    var csvString = csv.encode();

    // Use download.js to trigger the download.
    download(csvString, "Tweet.csv", "text/plain");
}

var downloadJson = function (json) {
    download(json, "Tweet.json", "text/plain");
}

var main = function () {

    var fetchButton = $('#fetch');
    var startInput = $('#start');
    var endInput = $('#end');
    var loading = $('#loading');
    var jsonCheckbox = $('#json')[0];
    var csvCheckbox = $('#csv')[0];

    fetchButton.click(function (e) {

        // Get start & end dates from the DOM
        var start = startInput.val();
        var end = endInput.val();

        if (end < start) {
            alert('start date must be greater than end!');
            return;
        }

        // Abort if no dates are selected
        if (!start || !end) {
            alert('Please select a start & end date!');
            return;
        }

        // Abort if the no export types are selected
        if (!jsonCheckbox.checked && !csvCheckbox.checked) {
            alert('Select an export type!');
            return;
        }

        // Build request URL
        var url = "/Home/GetData?start=" + start + "&end=" + end;

        // Show the loading screen
        loading.show();

        // Make the request.
        $.get(url, function (resultStr) {

            // Parse to into JS array.
            var tweets = JSON.parse(resultStr);

            // Tell user the bad news
            if (tweets.length == 0) {
                alert("No tweets on this date, sorry man :(");
                return;
            }

            // Trigger JSON file download
            if (jsonCheckbox.checked) {
                downloadJson(resultStr);
            }

            // Trigger CSV file download
            if (csvCheckbox.checked) {
                downloadCsv(tweets);
            }

            // Hide the loading icon
            loading.hide();
        });
    });
}

// Hook up main method to document ready event
$(document).ready(main);