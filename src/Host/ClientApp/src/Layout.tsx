import * as React from 'react';
import Header from './widgets/Header';
import { IsDesktop } from './features/MediaHelper';
import Sidebar from './widgets/Sidebar';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {
    const isDesktop = IsDesktop();
    const [isSidebarOpen, setIsSidebarOpen] = React.useState(false);

    const handleOpenSidebar = () => setIsSidebarOpen(true);
    const handleCloseSidebar = () => setIsSidebarOpen(false);

    return (
        <div className='h-screen w-full flex flex-col bg-dark overflow-hidden'>
            <Header className='w-full sticky top-0 flex-shrink-0 z-[1100]' onMenuClick={handleOpenSidebar} />
            <div className='flex-1 flex overflow-hidden'>
                {isDesktop ? (
                    <Sidebar className='h-full sticky top-0 flex-shrink-0 z-[1000] overflow-y-auto' />
                ) : (
                    <Sidebar isOpen={isSidebarOpen} onClose={handleCloseSidebar} />
                )}
                <main className='flex-1 h-full'>
                    {props.children}
                </main>
            </div>
        </div>
    );
};

export default Layout;