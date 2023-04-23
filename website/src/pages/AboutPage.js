import React, { useEffect, useState } from 'react';
import * as Blogs from './blogs';
import SpecificBlogPost from './blogs/SpecificBlogPost';

function AboutPage() {
    const [blogPosts, setBlogPosts] = useState([]);

    // Load all the blog posts
    useEffect(() => {
        const b = [];
        for (const key of Object.keys(Blogs)) {
            b.push(key);
        }

        // Sort in descending date order
        b.sort((a, b) => {
            const prefix = "Blog";
            let dateA = a.slice(a.indexOf(prefix) + prefix.length);
            let dateB = b.slice(b.indexOf(prefix) + prefix.length);
            dateA = dateA.slice(4) + dateA.slice(0, 4);
            dateB = dateB.slice(4) + dateB.slice(0, 4);
            return dateB - dateA;
        });

        setBlogPosts(b);

        // eslint-disable-next-line
    }, []);

    return (
        <div>
            <h1>About</h1>
            <p style={{marginTop: '-16px', color: 'grey'}}>Blogs about the project</p>
            <hr style={{ width: '80%' }} />

            <div>
                {blogPosts.map((v, i) => 
                <div key={i}>
                    {<SpecificBlogPost propFilename={v} isFullArticle={false}/>}
                </div>)}
            </div>
        </div>
    );
}

export default AboutPage;