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
    var jsonCheckbox = $('#json')[0];
    var csvCheckbox = $('#csv')[0];

    fetchButton.click(function (e) {

        // Get start & end dates from the DOM
        var start = startInput.val();
        var end = endInput.val();

        // Abort if no dates are selected
        if (!start || !end) {
            alert('Please select a start & end date!');
            return;
        }

        if (!jsonCheckbox.checked && !csvCheckbox.checked) {
            alert('Select an export type!');
        }

        // Build request URL
        var url = "/Home/GetData?start=" + start + "&end=" + end;

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
        });
    });
}

// Hook up main method to document ready event
$(document).ready(main);