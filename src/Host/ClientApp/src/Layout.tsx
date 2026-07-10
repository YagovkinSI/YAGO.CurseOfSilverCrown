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

    const content = () => {
        return (
            <div className='content-container'>
                {props.children}
            </div>
        )
    }

    return (
        <div className="min-h-screen bg-dark">
            <Header />
            <div className="flex pt-[68px] md:pt-[80px]">
                {isDesktop && <Sidebar />}
                <main className={`
                    flex-1 min-h-[calc(100vh-68px-56px)] md:min-h-[calc(100vh-80px)]
                `}>
                    {content()}
                </main>
            </div>
            {!isDesktop && <Footer />}
        </div>
    );
}

export default Layout;