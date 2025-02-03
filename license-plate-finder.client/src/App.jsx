import { useState } from "react";
import axios from "axios";

const App = () => {
    const [vehicle, setVehicle] = useState(null);
    const [error, setError] = useState(null);
    const [licensePlate, setLicensePlate] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const handleSearch = async () => {
        if (licensePlate.trim() === "") {
            setError("Please enter a license plate.");
            return;
        }

        setError(null);
        setIsLoading(true);
        try {
            const response = await axios.get(
                `http://localhost:5168/api/vehicle/${licensePlate}`
            );
            setVehicle(response.data);
        } catch {
            setError("Vehicle not found.");
            setVehicle(null);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className={"test"} style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', width: '100%', margin: 0 }}>
            <div style={{ maxWidth: '800px', margin: '0 auto', padding: '20px' }}>
                <h2 style={{ textAlign: 'center' }}>License Plate Finder</h2>
                <div style={{ display: 'flex', gap: '10px', justifyContent: 'center', marginTop: '20px' }}>
                    <input
                        type="text"
                        placeholder="Enter License Plate"
                        value={licensePlate}
                        onChange={(e) => setLicensePlate(e.target.value)}
                        style={{ width: '70%', padding: '10px', fontSize: '16px' }}
                    />
                    <button
                        onClick={handleSearch}
                        style={{
                            padding: '10px 20px',
                            fontSize: '16px',
                            backgroundColor: '#007bff',
                            color: 'white',
                            border: 'none',
                            cursor: 'pointer',
                            borderRadius: '4px',
                        }}
                    >
                        Search
                    </button>
                </div>
                {isLoading && <p style={{ textAlign: 'center', marginTop: '20px' }}>Loading...</p>}
                {error && (
                    <p style={{ color: 'red', textAlign: 'center', marginTop: '20px' }}>
                        {error}
                    </p>
                )}
                {vehicle && (
                    <div style={{ marginTop: '30px', padding: '20px', border: '1px solid #ccc', borderRadius: '4px' }}>
                        <h3>{vehicle.Brand}</h3>
                        <p><strong>Type:</strong> {vehicle.handelsbenaming}</p>
                        <p><strong>Brand:</strong> {vehicle.merk}</p>
                        <p><strong>Color:</strong> {vehicle.eerste_kleur}</p>
                        <p><strong>Doors:</strong> {vehicle.aantal_deuren}</p>
                        <p><strong>Engine Specifications: </strong></p>
                        <p><strong>Motorcode:</strong> {vehicle.motorcode}</p>
                        <p><strong>Horsepower:</strong> {vehicle.vermogen_motor_pk}</p>
                    </div>
                )}
            </div>
        </div>

    );
};

export default App;
