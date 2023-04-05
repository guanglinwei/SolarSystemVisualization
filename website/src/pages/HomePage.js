import { Button } from '@mui/material';
import React from 'react';
import { Link } from 'react-router-dom';

function HomePage() {
    return (
        <div>
            <h1>Solar System Visualization with Unity</h1>
            <hr style={{ width: '80%' }} />
            
            <div>
                <p>This is a solar system visualization made with Unity.</p>
                <p>The project is being done under the guidance of Professor Daniel Darg for the Astrophysics department at UMD.</p>
            </div>
            <div>
                <Button variant='outlined'>
                    <Link
                        to='/sim'
                        style={{ textDecoration: 'none' }}
                    >
                        View the simulation here
                    </Link>
                </Button>
            </div>
        </div>
    );
}

export default HomePage;