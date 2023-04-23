import React, { useEffect, useState } from 'react';
import * as Blogs from '.';
import { Link, useParams } from 'react-router-dom';

function SpecificBlogPost({ propFilename, isFullArticle }) {
    const { pathFilename } = useParams();
    const [fileName, setFileName] = useState("");
    const [BlogComponent, setBlogComponent] = useState(null);
    const [showGoBack, setShowGoBack] = useState(false);

    useEffect(() => {
        let f = propFilename || pathFilename;
        f = f?.charAt(0).toUpperCase() + f?.slice(1);
        setFileName(f);
    }, [pathFilename, propFilename]);

    useEffect(() => {
        if (!fileName || !Blogs[fileName]) {
            if (!!fileName) {
                setShowGoBack(isFullArticle);
                console.log(showGoBack);
            }
            setBlogComponent(null);
            return;
        }

        setShowGoBack(false);
        setBlogComponent(Blogs[fileName]({ isFullArticle: isFullArticle }));

        // eslint-disable-next-line
    }, [fileName, isFullArticle]);

    return (
        <>
            {BlogComponent ||
                (showGoBack && <div>
                    <h2>There's nothing here...</h2>
                    <Link to='/about'>Go back</Link>
                </div>)
            }
        </>
    );
}

export default SpecificBlogPost;