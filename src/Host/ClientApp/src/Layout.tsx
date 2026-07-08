import * as React from 'react';
import Header from './Header';
import Footer from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {
    const renderBackground = () => (
        <div
            className="absolute inset-0 -inset-x-[5px] -inset-y-[5px]"
            style={{
                backgroundColor: '#050515',
                backgroundImage: 'url("/images/background/bg.jpg")',
                backgroundSize: 'cover',
                backgroundPosition: 'center',
                backgroundRepeat: 'no-repeat',
            }}
        />
    );

    return (
        <div className="min-h-screen relative">
            <Header />
            
            <main className="relative pt-16 sm:pt-[66px] pb-8 sm:pb-10 min-h-screen overflow-hidden">
                {renderBackground()}
                
                <div className="relative z-[150] h-full w-full p-2 md:p-4 overflow-y-auto">
                    <div className="flex items-center justify-center min-h-[calc(100vh-80px-40px)] sm:min-h-[calc(100vh-100px-50px)]">
                        {props.children}
                    </div>
                </div>
            </main>

            <Footer />
        </div>
    );
};

export default Layout;