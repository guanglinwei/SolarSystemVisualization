import './App.css';
import { Routes, Route, Outlet, useLocation, useNavigate, HashRouter } from 'react-router-dom';
import HomePage from './pages/HomePage';
import TopAppBar from './components/TopAppBar';
import AboutPage from './pages/AboutPage';
import SimulationPage from './pages/SimulationPage';
import { Box } from '@mui/material';
import { useUnityContext } from 'react-unity-webgl';

function App() {
    return (
        <div className='App'>
            <HashRouter>
                <RoutesComponent />
            </HashRouter>
        </div>
    );
}

const pages = [
    { path: "/", name: "Home" },
    { path: "/about", name: "About" },
    { path: "/sim", name: "Simulation" }
];

function RoutesComponent() {
    const location = useLocation();
    const navigate = useNavigate();

    const { unityProvider, unload } = useUnityContext({
        loaderUrl: `${process.env.PUBLIC_URL}/UnityBuild/Build/dist.loader.js`,
        dataUrl: `${process.env.PUBLIC_URL}/UnityBuild/Build/dist.data.unityweb`,
        frameworkUrl: `${process.env.PUBLIC_URL}/UnityBuild/Build/dist.framework.js.unityweb`,
        codeUrl: `${process.env.PUBLIC_URL}/UnityBuild/Build/dist.wasm.unityweb`
    });

    async function needToUnloadCallback(path) {
        if(unload) {
            await unload();
            navigate(path);
        }
    }

    return (
        <Box sx={{ display: 'flex', maxHeight: '100%', height: '100vh', flexDirection: 'column' }}>
            <Routes>
                <Route exact path='/' element={<>
                    <TopAppBar pages={pages}
                        unloadBeforeRedirect={location.pathname === '/sim'}
                        needToUnloadCallback={needToUnloadCallback} />
                    <Outlet />
                </>}>
                    <Route index element={<HomePage />} />
                    <Route path='/about' element={<AboutPage />} />
                    <Route path='/sim' element={<SimulationPage unityProvider={unityProvider} />} />
                </Route>
            </Routes>
        </Box>
    );
}

export default App;