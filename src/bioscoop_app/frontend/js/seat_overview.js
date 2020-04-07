function loadSeatOverview() {
    let room = rooms[selectedRoom];
    let seatTypes = ['normal', 'luxery', 'vip'];

    for (let row = 0; row < room.length; row++) {
        let elRow = document.createElement('div');
        elRow.classList.add('row', 'row-' + row);
        elRow.style.height = blockSize + 'px';
        container.appendChild(elRow);

        for (let seat = 0; seat < room[row].length; seat++) {
            let seatType = room[row][seat];

            if (seatType != 0) {
                let elSeat = document.createElement('div');
                elSeat.classList.add('seat', 'seat-' + seat, seatTypes[seatType - 1]);
                elSeat.style.left = (seat * blockSize) + 'px';
                elSeat.style.width = blockSize + 'px';

                elRow.appendChild(elSeat);
            }
        }
    }
}

// Settings for prototype/demo
let rooms = [[
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
    [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0]]
];
let container = document.querySelector('#cinema-room');
let selectedRoom = 0;
let blockSize = 15;

loadSeatOverview();