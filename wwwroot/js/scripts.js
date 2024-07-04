document.addEventListener('DOMContentLoaded', function () {
    const apiUrl = 'https://localhost:5001/api';

    async function loadPets() {
        try {
            const response = await fetch(`${apiUrl}/pets`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log('Pets loaded:', data);

            const pets = data.$values;
            const petsTableBody = document.querySelector('#petsTable tbody');
            petsTableBody.innerHTML = '';
            pets.forEach(pet => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${pet.name}</td>
                    <td>${pet.species}</td>
                    <td>${pet.age}</td>
                    <td>${pet.shelter.name}</td>
                    <td>
                        <button class="btn btn-danger btn-sm" onclick="deletePet(${pet.petId})">Delete</button>
                    </td>
                `;
                petsTableBody.appendChild(row);
            });
        } catch (error) {
            console.error('Error loading pets:', error);
        }
    }

    async function loadShelters() {
        try {
            const response = await fetch(`${apiUrl}/shelters`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log('Shelters loaded:', data);

            const shelters = data.$values;
            const sheltersTableBody = document.querySelector('#sheltersTable tbody');
            sheltersTableBody.innerHTML = '';
            shelters.forEach(shelter => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${shelter.name}</td>
                    <td>${shelter.location}</td>
                    <td>${shelter.contactInfo}</td>
                    <td>
                        <button class="btn btn-danger btn-sm" onclick="deleteShelter(${shelter.shelterId})">Delete</button>
                    </td>
                `;
                sheltersTableBody.appendChild(row);
            });

            const shelterSelect = document.getElementById('petShelter');
            shelterSelect.innerHTML = '<option value="">Select Shelter</option>';
            shelters.forEach(shelter => {
                const option = document.createElement('option');
                option.value = shelter.shelterId;
                option.textContent = shelter.name;
                shelterSelect.appendChild(option);
            });
        } catch (error) {
            console.error('Error loading shelters:', error);
        }
    }

    document.getElementById('createPetButton').addEventListener('click', function () {
        $('#createPetModal').modal('show');
    });

    document.getElementById('createShelterButton').addEventListener('click', function () {
        $('#createShelterModal').modal('show');
    });

    document.getElementById('createPetForm').addEventListener('submit', async function (e) {
        e.preventDefault();
        const pet = {
            name: document.getElementById('petName').value,
            species: document.getElementById('petSpecies').value,
            age: parseInt(document.getElementById('petAge').value),
            shelterId: parseInt(document.getElementById('petShelter').value)
        };

        console.log('Pet to create:', pet);

        try {
            const response = await fetch(`${apiUrl}/pets`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(pet)
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.log('Error response from server:', errorData);
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            console.log('Pet created successfully:', data);

            $('#createPetModal').modal('hide');
            loadPets();
        } catch (error) {
            console.error('Error creating pet:', error);
        }
    });

    document.getElementById('createShelterForm').addEventListener('submit', async function (e) {
        e.preventDefault();
        const shelter = {
            name: document.getElementById('shelterName').value,
            location: document.getElementById('shelterLocation').value,
            contactInfo: document.getElementById('shelterContactInfo').value
        };

        console.log('Shelter to create:', shelter);

        try {
            const response = await fetch(`${apiUrl}/shelters`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(shelter)
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.log('Error response from server:', errorData);
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            console.log('Shelter created successfully:', data);

            $('#createShelterModal').modal('hide');
            loadShelters();
        } catch (error) {
            console.error('Error creating shelter:', error);
        }
    });

    window.deletePet = async function (petId) {
        try {
            const response = await fetch(`${apiUrl}/pets/${petId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            console.log(`Pet with ID ${petId} deleted successfully.`);
            loadPets();
        } catch (error) {
            console.error(`Error deleting pet with ID ${petId}:`, error);
        }
    };

    window.deleteShelter = async function (shelterId) {
        try {
            const response = await fetch(`${apiUrl}/shelters/${shelterId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            console.log(`Shelter with ID ${shelterId} deleted successfully.`);
            loadShelters();
        } catch (error) {
            console.error(`Error deleting shelter with ID ${shelterId}:`, error);
        }
    };

    // Initial load
    loadPets();
    loadShelters();
});
