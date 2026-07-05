import React from 'react';
import { GetFooterHeight, GetHeaderHeight, IsDesktop } from '../features/MediaHelper';

interface PageContainerProps {
    children: React.ReactNode;
    justifyContent?: string;
    backgroundImage?: string;
    darkenBackground?: boolean;
    hideHeader?: boolean;
    hideFooter?: boolean;
    className?: string;
}

const PageContainer: React.FC<PageContainerProps> = ({
    children,
    justifyContent = 'center',
    backgroundImage = 'space',
    darkenBackground = false,
    hideHeader = false,
    hideFooter = false,
    className = '',
}) => {
    const isDesktop = IsDesktop();
    const headerHeight = GetHeaderHeight(hideHeader);
    const footerHeight = GetFooterHeight(hideFooter);

    return (
        <div
            className={`
                fixed inset-0
                bg-cover bg-center bg-fixed
                ${isDesktop ? 'ml-64' : ''}
                ${className}
            `}
            style={{
                backgroundImage: `url('/images/pictures/${backgroundImage}.jpg')`
            }}
        >
            {darkenBackground && (
                <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]" />
            )}

            <div
                className="absolute inset-0 overflow-y-auto overscroll-contain"
                style={{
                    top: `${headerHeight}px`,
                    bottom: `${footerHeight}px`,
                    left: '16px',
                    right: '16px',
                }}
            >
                <div 
                    className="flex flex-col items-center justify-center w-full min-h-full py-2"
                    style={{ justifyContent: `${justifyContent}` }}
            >
                    {children}
                </div>
            </div>
        </div>
    );
};

export default PageContainer;