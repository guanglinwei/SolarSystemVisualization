import React from 'react';
import { AppBar, Toolbar, Box, Button } from '@mui/material';
import { Link } from 'react-router-dom';

function TopAppBar({ pages }) {
    return (
        <AppBar position='sticky' sx={{ backgroundColor: '#2d2d2d' }}>
            <Toolbar>
                <Box sx={{ flexGrow: 1, display: 'flex' }}>
                    {pages.map((page) => (
                        <Link key={page.name} to={page.path || '/'} style={{ textDecoration: 'none' }}>
                            <Button sx={{ color: '#fff' }}>
                                {page.name}
                            </Button>
                        </Link>
                    ))}
                </Box>
            </Toolbar>
        </AppBar>
    );
}

export default TopAppBar;