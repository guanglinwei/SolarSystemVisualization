import { Box, Card, CardContent, Grid } from '@mui/material';
import React from 'react';
import { Link } from 'react-router-dom';

function BlogPost({ title, date, isFullArticle = true, filename, imagePath, imageAlt, children }) {
    return (
        <div>
            {isFullArticle
                ? <div>
                    <h2 style={{ marginBottom: '4px' }}>{title}</h2>
                    <div style={{ color: 'grey' }}>{date}</div>
                    <hr style={{ width: '80%' }} />
                    <div>
                        {children}
                    </div>
                </div>
                : <Card sx={{ width: '50%', margin: 'auto', marginTop: '10px' }}>
                    <Link to={'/blog/' + filename} style={{ textDecoration: 'none', color: 'black' }}>
                        <Grid container justifyContent={'center'} alignItems={'center'}>
                            <Grid item xs={6}>
                                <Box>
                                    <img src={process.env.PUBLIC_URL + '/' + (imagePath || '/blogs/earthicon.svg')}
                                        alt={imagePath ? (imageAlt || '') : 'Earth icon'}
                                        style={{ maxHeight: '96px' }} />
                                </Box>
                            </Grid>
                            <Grid item xs={6}>
                                <Box>
                                    <CardContent>
                                        <h3>{title}</h3>
                                        <p>{date}</p>
                                    </CardContent>
                                </Box>
                            </Grid>
                        </Grid>
                    </Link>
                </Card>}
        </div>
    );
}

export default BlogPost;