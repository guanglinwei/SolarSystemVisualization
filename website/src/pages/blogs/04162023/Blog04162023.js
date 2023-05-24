import React from 'react';
import BlogPost from '../BaseBlogPost';

function Blog03262023(props) {
    return (
        <div>
            <BlogPost title={'Setting up the Planets'} date={'April 16, 2023'} filename={'Blog04162023'} {...props}>
                <div>
                    <p>
                        To ensure the simulation works properly, we need some data about the planets
                        &#40;mass, radius, distance from the sun, and velocity&#41;. This data can be
                        found <a href='https://nssdc.gsfc.nasa.gov/planetary/factsheet/'>here</a>.
                        However, there are a few issues.
                    </p>
                    <p>
                        The distance between planets is significantly larger than the radius of any planet. This means
                        that if the simulation was scaled properly, it would be very difficult to see anything.
                        To fix this, the distance between planets was scaled down by a factor of 10^6 km.
                    </p>
                    <p>
                        Doing so raises another problem: the sun is too large and cover some of the closer planets.
                        As a result, the sun has been scaled down to much smaller than it actually is.
                    </p>
                    <div style={{ textAlign: 'center' }}>
                        <img src={process.env.PUBLIC_URL + '/blogs/05242023sceneview.png'} alt='Unity Scene View'
                            width='70%' />
                    </div>
                </div>
            </BlogPost>
        </div>
    );
}

export default Blog03262023;