import React from 'react';
import BlogPost from '../BaseBlogPost';

function Blog08062023(props) {
    return (
        <div>
            <BlogPost title={'Adding the UI'} date={'August 6, 2023'} filename={'Blog08062023'} {...props}>
                <div>
                    <p>
                        The simulation needs some better UI. The first thing to add is the ability to focus on
                        and zoom onto different planets. Typing into the upper left search bar allows this.
                    </p>
                    <p>
                        The simulation also needs some more info to make it useful as an educational tool. I added
                        a menu to the right that pops up whenever the user selects a planet. This menu currently shows
                        stats about the planet. In the future, other fun facts could be shown here as well.
                    </p>
                    <div style={{ textAlign: 'center' }}>
                        <img src={process.env.PUBLIC_URL + '/blogs/08062023ui.png'} alt='Simulation UI'
                            width='70%' />
                    </div>
                </div>
            </BlogPost>
        </div>
    );
}

export default Blog08062023;