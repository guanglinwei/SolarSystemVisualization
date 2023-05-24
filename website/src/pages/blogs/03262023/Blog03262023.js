import React from 'react';
import BlogPost from '../BaseBlogPost';
import { Grid } from '@mui/material';

function Blog03262023(props) {
    return (
        <div>
            <BlogPost title={'Why Unity?'} date={'March 26, 2023'} filename={'Blog03262023'}
                imagePath={'blogs/unitylogo.svg'} imageAlt={'Unity Logo'} {...props}>
                <div>
                    <p>
                        When I started this project, I had to decide between 3 different rendering engines:
                        <span> <a href='https://unity.com/'>Unity</a>, </span>
                        <span><a href='https://godotengine.org/'>Godot</a>, or </span>
                        <span><a href='https://threejs.org/'>Three.js</a></span>
                    </p>
                    <p>
                        I've had brief experiences with all three of these, but I was most familiar with Unity.
                        These were some of the other factors I considered: <br />
                    </p>
                    <div>
                        <ul>
                            <li>Unity has the most in-depth documentation and the most users</li>
                            <li>
                                Godot and Three.js are completely free, while Unity requires a paid license
                                if you make more than $100k annually &#40;which is very unlikely for this project&#41;
                            </li>
                            <li>There is general consensus that Unity is better than Godot for 3D physics and rendering</li>
                            <li>
                                Three.js would make it very easy to embed the simulation in a website. Both Godot and Unity
                                can export to WebGL, but integrating it into a website will take extra effort.
                            </li>
                            <li>Unity's popularity makes learning it a more useful skill than learning Godot or Three.js</li>
                        </ul>
                    </div>
                    <p>I ultimately decided to go with Unity because of my previous experience, ease of use, and its popularity.</p>
                    <div>
                        <Grid container justifyContent={'center'} alignItems={'center'} textAlign={'center'}>
                            <Grid item xs={4}>
                                <img src={process.env.PUBLIC_URL + '/blogs/unitylogo.svg'} alt='Unity Logo' width={'50%'} />
                            </Grid>
                            <Grid item xs={4}>
                                <img src={process.env.PUBLIC_URL + '/blogs/godotlogo.svg'} alt='Godot Logo' width={'50%'} />
                            </Grid>
                            <Grid item xs={4}>
                                <img src={process.env.PUBLIC_URL + '/blogs/threejslogo.svg'} alt='Three.js Logo' width={'50%'} />
                            </Grid>
                        </Grid>
                    </div>
                </div>
            </BlogPost>
        </div>
    );
}

export default Blog03262023;