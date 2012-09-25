var map;
var polylineArray = new Array();    // Use this to keep track of the lines added

function initialize() {
    var mapDiv = document.getElementById('map-canvas');
    map = new google.maps.Map(mapDiv, {
        center: new google.maps.LatLng(38.06827, -98.59313),
        zoom: 3,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    });

    // add a get bounds method to the polyline
    google.maps.Polyline.prototype.getBounds = function () {
        var bounds = new google.maps.LatLngBounds();
        this.getPath().forEach(function (e) {
            bounds.extend(e);
        });
        return bounds;
    };

    // create a custom control that when clicked will re-center the view of the map to the polylines
    var centerControlDiv = document.createElement('div');
    var reCenterCtrl = new CenterMapControl(centerControlDiv);
    map.controls[google.maps.ControlPosition.TOP_RIGHT].push(centerControlDiv);
}

function CenterMapControl(controlDiv) {
    // Set CSS styles for the DIV containing the control
    // Setting padding to 5 px will offset the control
    // from the edge of the map.
    controlDiv.style.padding = '5px';

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'white';
    controlUI.style.borderStyle = 'solid';
    controlUI.style.borderWidth = '2px';
    controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Click to set the map to Home';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '14px';
    controlText.style.paddingLeft = '4px';
    controlText.style.paddingRight = '4px';
    // This is a home picture that will be the icon for the button
    controlText.innerHTML = '<img src="home.png" height="17" width="15" />';
    controlUI.appendChild(controlText);

    // Setup the click event listeners:
    google.maps.event.addDomListener(controlUI, 'click', function () {
        var bounds = new google.maps.LatLngBounds();
        for (var key in polylineArray) {
            if (polylineArray[key] != null) {
                var path = polylineArray[key].getPath();
                for (var j = 0; j < path.getLength(); j++) {
                    bounds.extend(path.getAt(j));
                }
            }
        }
        if (!bounds.isEmpty()) {
            map.fitBounds(bounds);
        }
        else {
            // reset map to original view
            map.setCenter(new google.maps.LatLng(38.06827, -98.59313));
            map.setZoom(3);
        }
    });
}

function addPoints(latitudes, longitudes, color, idx) {
    // Eval all paramaters (might need to use JSON for security)
    var lats = eval(latitudes);
    var longs = eval(longitudes);
    //TODO don't move the map when adding 2nd track
    var bounds;
    var path = new Array();

    // Assign the coordinats to the path
    if (!polylineArray[idx]) {
        bounds = new google.maps.LatLngBounds();
        for (i = 0; i < lats.length; i++) {
            path[i] = new google.maps.LatLng(lats[i], longs[i]);
            bounds.extend(path[i]);
        }
        polylineArray[idx] = new google.maps.Polyline({
            path: path,
            strokeColor: color,
            strokeOpacity: 1.0,
            strokeWeight: 2,
            geodesic: true
        });
    }
    // Else, the line already exists
    // Check if it is set to the map
    if (!polylineArray[idx].getMap()) {
        polylineArray[idx].setMap(map)
    }
    if (bounds) {
        map.fitBounds(bounds);
    }
}

function removeLine(key) {
    try {
        polylineArray[key].setMap(null);
    }
    catch (err) {
        alert(err);
    }
}

function changeColor(key, color) {
    try {
        polylineArray[key].setOptions({ strokeColor: color });
    }
    catch (err) {   // usually this error is a result of the lap not being on the map yet
        //alert(err);
    }
}

function clearPolyArray() {
    // remove all the items from the polyline array
    polylineArray = [];
}

google.maps.event.addDomListener(window, 'load', initialize);