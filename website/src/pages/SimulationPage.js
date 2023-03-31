import React from 'react';
import UnityBuild from "../UnityBuild/index.html";

function SimulationPage() {
    return (
        <div>
            <h1>Simulation</h1>
            <hr style={{ width: "80%" }} />
            <iframe src={UnityBuild} title="simulation"></iframe>
        </div>
    );
}

export default SimulationPage;