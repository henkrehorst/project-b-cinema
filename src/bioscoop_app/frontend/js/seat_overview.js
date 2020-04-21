﻿function setStyle(el, styles = {}) {
    for (let style in styles) {
        el.style[style] = styles[style];
    }
}

function resetSeatOverview() {
    gridContainer.innerHTML = '<div id="cinema-room"></div>';
    container = document.querySelector('#cinema-room');
    maxColLength = rooms[selectedRoom][0].length;
}

function sendError(errorMsg = '') {
    let elError = document.querySelector('.error-msg');

    elError.innerText = errorMsg;
    elError.classList.add('warning');
    setTimeout(function () {
        elError.classList.remove('warning');
    }, 2000);
}

function isAdjacent(newSelected, allSelected = []) {
    for (let i = 0; i < allSelected.length; i++) {
        if (allSelected[i].seat == (newSelected - 1) || allSelected[i].seat == (newSelected + 1)) {
            return true;
        }
    }

    return allSelected.length > 0 ? false : true;
}

function updateOrderText() {
    let orderedText = '';
    let totalPrice = 0;
    let room = rooms[selectedRoom];

    for (let i = 0; i < selectedSeats.length; i++) {
        orderedText += '\nRij ' + (room.length - selectedSeats[i].row) + ', stoel ' + (selectedSeats[i].seat + 1) + ' (' + selectedSeats[i].type + ' ' + selectedSeats[i].price + '$).';
        totalPrice += selectedSeats[i].price;
    }

    let seatDescription = (selectedSeats.length) + ' stoel' + (selectedSeats.length != 1 ? 'en ' : ' ') + (selectedSeats.length > 0 ? 'geselecteerd: ' : 'geselecteerd.') + orderedText + '\nTotale prijs: \u20AC' + (Math.round(totalPrice * 100) / 100);
    document.querySelector('.seat-description').innerText = seatDescription;
}

function loadSeatOverview() {
    let room = rooms[selectedRoom];
    let gridColumn = document.createElement('div');
    let gridRow = document.createElement('div');
    let gridAvailable = document.createElement('div');

    gridColumn.classList.add('grid-column');
    gridRow.classList.add('grid-row');
    gridAvailable.classList.add('grid-available');

    // Grid columns
    for (let column = 0; column < maxColLength; column++) {
        let elSpan = document.createElement('span');
        let txt = document.createTextNode(column + 1);

        elSpan.appendChild(txt);
        elSpan.classList.add('grid-number');
        setStyle(elSpan, { 'width': blockSize + 'px', 'height': blockSize + 'px', left: (blockSize * column + padding / 2 + blockSize) + 'px', 'top': '0px', 'font-size': (15 + blockSize * 3) + '%', 'line-height': blockSize + 'px' });
        gridColumn.appendChild(elSpan);
    }

    // Grid rows
    for (let row = 0; row < room.length; row++) {
        let elSpan = document.createElement('span');
        let txt = document.createTextNode(room.length - row);

        elSpan.appendChild(txt);
        elSpan.classList.add('grid-number');
        setStyle(elSpan, { 'width': blockSize + 'px', 'height': blockSize + 'px', 'left': '0px', 'top': (blockSize * row + blockSize + padding / 2) + 'px', 'font-size': (15 + blockSize * 3) + '%', 'line-height': (blockSize) + 'px' });
        gridRow.appendChild(elSpan);
    }

    // Grid available seats
    let totalAvailable = 0;

    for (let column = 0; column <= maxColLength; column++) {
        let elSpan = document.createElement('span');
        let txt;
        let offset = 0;

        if (column != maxColLength) {
            let availableSeats = 0;

            for (let row = 0; row < room.length; row++) {
                if (room[row][column] != 0) {
                    availableSeats++;
                }
            }

            totalAvailable += availableSeats;
            txt = document.createTextNode(availableSeats);
        }
        else {
            txt = document.createTextNode(totalAvailable);
            offset = 25;
        }

        elSpan.appendChild(txt);
        elSpan.classList.add('grid-number');
        setStyle(elSpan, { 'width': blockSize + 'px', 'height': blockSize + 'px', left: (blockSize * column + padding / 2 + blockSize + offset) + 'px', 'bottom': (-blockSize - padding / 2 - 50) + 'px', 'font-size': (15 + blockSize * 3) + '%', 'line-height': blockSize + 'px' });
        gridAvailable.appendChild(elSpan);
    }
     
    gridContainer.appendChild(gridColumn);
    gridContainer.appendChild(gridRow);
    gridContainer.appendChild(gridAvailable);
    setStyle(container, { 'width': (blockSize * maxColLength + padding) + 'px', 'height': (blockSize * room.length + padding) + 'px', 'top': (blockSize / 1.75 + 8 * (blockSize / 20)) + 'px', 'left': (blockSize) + 'px' });
    setStyle(document.querySelector('.screen-title'), { 'width': (blockSize * maxColLength + padding) + 'px' });
    setStyle(document.querySelector('.controls'), { 'margin-left': (blockSize + 15) + 'px' });

    // Seat overview
    for (let row = 0; row < room.length; row++) {
        for (let seat = 0; seat < room[row].length; seat++) {
            let seatType = room[row][seat];

            if (seatType != 0) {
                let elSeat = document.createElement('div');
                elSeat.classList.add('seat', 'row-' + row, 'seat-' + seat, seatTypes[seatType - 1]);
                setStyle(elSeat, {
                    'top': (row * blockSize + (padding / 2)) + 'px',
                    'left': (seat * blockSize + (padding / 2)) + 'px',
                    'width': blockSize + 'px',
                    'height': blockSize + 'px'
                });

                for (let i = 0; i < selectedSeats.length; i++) {
                    if (selectedSeats[i].row == row && selectedSeats[i].seat == seat) {
                        elSeat.classList.add('selected');
                    }
                }
                
                container.appendChild(elSeat);

                // Seat click event
                elSeat.addEventListener('click', (event, selectedRow = row, selectedSeat = seat, type = seatType) => {
                    let price = seatPrices[type - 1];
                    let selected = document.querySelector('.seat-' + selectedSeat + '.row-' + selectedRow);

                    if (selected.classList.contains('selected')) {
                        selected.classList.remove('selected');

                        for (let i = 0; i < selectedSeats.length; i++) {
                            if (selectedSeats[i].row == selectedRow && selectedSeats[i].seat == selectedSeat) {
                                selectedSeats.splice(i, 1);
                            }
                        }
                    }
                    else {
                        if (!selectedSeats.length || selectedSeats[0].row == selectedRow) {
                            if (isAdjacent(selectedSeat, selectedSeats)) {
                                selected.classList.add('selected');
                                selectedSeats.push({ 'row': selectedRow, 'seat': selectedSeat, 'type': (type == 1 ? 'normaal' : (type == 2 ? 'luxe' : 'VIP')), 'price': price });
                            }
                            else {
                                sendError('Je kan alleen stoelen naast je geselecteerde stoelen selecteren!');
                            }
                        }
                        else {
                            sendError('Je kan alleen stoelen van dezelfde rij selecteren!');
                        }
                    }

                    updateOrderText();
                });

                // Seat hover events
                elSeat.addEventListener('mouseenter', (event) => {
                    let target = event.target;
                    let targetRow = 0;
                    let targetCol = 0;
                    
                    for (let i = 0; i < target.classList.length; i++) {
                        if (target.classList[i].substring(0, 4) == 'row-') {
                            targetRow = parseInt(target.classList[i].substring(4, target.classList[i].length));
                        }
                        else if (target.classList[i].substring(0, 5) == 'seat-') {
                            targetCol = parseInt(target.classList[i].substring(5, target.classList[i].length));
                        }
                    }

                    for (let row = 0; row < room.length; row++) {
                        for (let column = 0; column < maxColLength; column++) {
                            let seat = document.querySelector('.seat.row-' + row + '.seat-' + column);

                            if (seat) {
                                if (targetCol == column || targetRow == row) {
                                    let highlight = document.querySelector('.seat.row-' + row + '.seat-' + column);
                                    let gridRow = document.querySelectorAll('.grid-row .grid-number')[row];
                                    let gridCol = document.querySelectorAll('.grid-column .grid-number')[column];

                                    highlight.classList.add('highlighted');

                                    if(targetRow == row) gridRow.classList.add('highlighted');
                                    else if(targetCol == column) gridCol.classList.add('highlighted');
                                }
                            }
                        }
                    }
                });

                elSeat.addEventListener('mouseleave', (event) => {
                    let highlighted = document.querySelectorAll('.highlighted');

                    for (let i = 0; i < highlighted.length; i++) {
                        highlighted[i].classList.remove('highlighted');
                    }
                });
            }
        }
    }
}

// Controls: zoom-in, zoom-out & load new room
document.querySelector('.controls button.zoom-in').addEventListener('click', () => {
    blockSize < 30 ? blockSize += 5 : blockSize = 30;
    resetSeatOverview();
    loadSeatOverview();
});
document.querySelector('.controls button.zoom-out').addEventListener('click', () => {
    blockSize > 10 ? blockSize -= 5 : blockSize = 10;
    resetSeatOverview();
    loadSeatOverview();
});
document.querySelector('.controls button.next-screen').addEventListener('click', () => {
    selectedRoom < 2 ? selectedRoom++ : selectedRoom = 0;
    selectedSeats = [];
    document.querySelector('.screen-title').innerText = 'SCHERM ' + (selectedRoom + 1);
    updateOrderText();
    resetSeatOverview();
    loadSeatOverview();
});

// Settings for prototype/demo (All can easely be adjusted by the back-end, I made everything dynamic)

// Below is a 2 dimensional array of all rooms, you can close this variable to save space (Press the - icon, left from the variable)
let rooms = [
    // Screen auditorium 1
    [
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0]
    ],

    // Screen auditorium 2
    [
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 1, 1, 1, 0],

        [1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],

        [0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0],

        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0]
    ],

    // Screen auditorium 3
    [
        [0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0],

        [0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0],
        [1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1],

        [1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1],
        [0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0],
    ]
];
let gridContainer = document.querySelector('#cinema-grid'); // The container that surrounds the grid and seats
let container = document.querySelector('#cinema-room'); // The container that surrounds the seats overview
let selectedRoom = 2; // Current selected room (0 is small, 1 is medium and 2 is the big room)
let blockSize = 20; // Starting size of each seat block
let padding = 20; // Padding that surrounds the cinema room container
let seatPrices = [7.99, 12.99, 17.99]; // Prices of each seat
let seatTypes = ['normal', 'luxery', 'vip']; // Type of each seat (Can be merged with the array above but I'm to lazy)
let selectedSeats = []; // All newly selected seats will be stored inside this array
let maxColLength = rooms[selectedRoom][0].length; // Defines the max amount of columns in a row (default row 0)

loadSeatOverview();