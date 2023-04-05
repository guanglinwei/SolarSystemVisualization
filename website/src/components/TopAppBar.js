import React from 'react';
import { AppBar, Toolbar, Box, Button } from '@mui/material';
import { Link } from 'react-router-dom';
import GithubIcon from '@mui/icons-material/GitHub';

function TopAppBar({ pages, unloadBeforeRedirect = false, needToUnloadCallback = undefined }) {
    function handleUnload(event, path) {
        event.preventDefault();
        needToUnloadCallback(path);
    }
    return (
        <AppBar position='sticky' sx={{ backgroundColor: '#2d2d2d' }}>
            <Toolbar>
                <Box sx={{ flexGrow: 1, display: 'flex' }}>
                    {pages.map((page) => (
                        <Link
                            onClick={unloadBeforeRedirect ? (event) => handleUnload(event, page.path || '/') : undefined}
                            key={page.name} to={page.path || '/'}
                            style={{ textDecoration: 'none' }}
                        >
                            <Button sx={{ color: '#fff' }}>
                                {page.name}
                            </Button>
                        </Link>
                    ))}
                </Box>
                <Box sx={{ flexGrow: 0 }}>
                    <a href='https://github.com/guanglinwei/SolarSystemVisualization' style={{ textDecoration: 'none' }}>
                        <GithubIcon fontSize='large' sx={{ color: 'white' }}/>
                    </a>
                </Box>
            </Toolbar>
        </AppBar>
    );
}

export default TopAppBar;