import './App.css';
import { BrowserRouter, Routes, Route, Outlet, useLocation, useNavigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import TopAppBar from './components/TopAppBar';
import AboutPage from './pages/AboutPage';
import SimulationPage from './pages/SimulationPage';
import { Box } from '@mui/material';
import { useUnityContext } from 'react-unity-webgl';

function App() {
    return (
        <div className='App'>
            <BrowserRouter>
                <RoutesComponent />
            </BrowserRouter>
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
        loaderUrl: '/UnityBuild/Build/dist.loader.js',
        dataUrl: '/UnityBuild/Build/dist.data.unityweb',
        frameworkUrl: 'UnityBuild/Build/dist.framework.js.unityweb',
        codeUrl: 'UnityBuild/Build/dist.wasm.unityweb'
    });

    async function needToUnloadCallback(path) {
        await unload();
        navigate(path);
    }

    return (
        <Box sx={{ display: 'flex', maxHeight: '100%', height: '100vh', flexDirection: 'column' }}>
            <Routes>
                <Route path='/' element={<>
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
