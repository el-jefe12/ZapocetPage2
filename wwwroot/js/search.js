function search(inputId, rowClass, searchColumns) {
    // Get input value
    var input = document.getElementById(inputId).value.toUpperCase();

    // Get rows
    var rows = document.querySelectorAll("." + rowClass);

    // Loop through rows and show/hide based on search input
    for (var i = 0; i < rows.length; i++) {
        // Skip the header row
        if (rows[i].classList.contains('header')) {
            continue;
        }

        var showRow = false;
        for (var j = 0; j < searchColumns.length; j++) {
            var column = rows[i].getElementsByClassName(searchColumns[j])[0];
            if (column && column.textContent.toUpperCase().includes(input)) {
                showRow = true;
                break;
            }
        }
        var hrElement = rows[i].previousElementSibling;
        rows[i].style.display = showRow ? "" : "none";
        if (hrElement && hrElement.tagName === 'HR') {
            hrElement.style.display = showRow ? "" : "none";
        }
    }
}
