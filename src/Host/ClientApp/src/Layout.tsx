import * as React from 'react';
import Header from './widgets/Header';
import Footer from './widgets/Footer';
import { IsDesktop } from './features/MediaHelper';
import Sidebar from './widgets/Sidebar';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {
    const isDesktop = IsDesktop();

    const renderMiddlePart = () => (
        <div className='flex flex-1 overflow-hidden'>
            {isDesktop && <Sidebar />}
            <main className='flex-1 overflow-y-auto'>
                {props.children}
            </main>
        </div>
    )

    return (
        <div className='h-screen bg-dark flex flex-col overflow-hidden'>
            <Header />
            {renderMiddlePart()}
            {!isDesktop && <Footer />}
        </div>
    );
};

export default Layout;