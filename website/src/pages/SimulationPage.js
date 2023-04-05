import { Box } from '@mui/material';
import React from 'react';
import { useLocation } from 'react-router-dom';
import { Unity } from 'react-unity-webgl';

function SimulationPage({ unityProvider }) {
    const location = useLocation();

    return (
        <Box flexGrow={1} sx={{ display: 'flex', flexDirection: 'column' }}>
            <h1>Simulation</h1>
            <hr style={{ width: '80%' }} />
            <Box style={{ flexGrow: 1 }}>
                {location.pathname === '/sim' ?
                    <Unity unityProvider={unityProvider} style={{ maxWidth: '95%', height: '75vh' }} />
                    : <></>
                }
            </Box>
        </Box>
    );
}

export default SimulationPage;