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
            <Header onMenuClick={handleOpenSidebar} className='w-full sticky top-0 flex-shrink-0 z-[1100]' />
            <div className='flex-1 flex overflow-hidden'>
                <Sidebar 
                    isOpen={isDesktop ? undefined : isSidebarOpen } 
                    onClose={isDesktop ? undefined: handleCloseSidebar } 
                    className={isDesktop
                        ? 'h-full sticky top-0 flex-shrink-0 z-[1000] overflow-y-auto'
                        : 'fixed top-0 left-0 h-full z-[1200] shadow-2xl transform transition-transform duration-300'
                    }/>
                <main className='flex-1 h-full'>
                    {props.children}
                </main>
            </div>
        </div>
    );
};

export default Layout;