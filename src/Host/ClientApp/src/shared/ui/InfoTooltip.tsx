import React, { useEffect, useLayoutEffect, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';

export interface InfoTooltipContent {
    name: string;
    imageName: string | null;
    description: string[];
}

interface InfoTooltipProps {
    content: InfoTooltipContent;
    children: React.ReactNode;
}

const InfoTooltip: React.FC<InfoTooltipProps> = ({ content, children }) => {
    const triggerRef = useRef<HTMLSpanElement>(null);
    const tooltipRef = useRef<HTMLDivElement>(null);
    const [open, setOpen] = useState(false);
    const [position, setPosition] = useState({ top: 0, left: 0 });

    useEffect(() => {
        if (!open) return;
        const closeOnOutside = (e: MouseEvent) => {
            const el = e.target as HTMLElement;
            if (triggerRef.current?.contains(el)) return;
            setOpen(false);
        };
        const closeOnScroll = () => setOpen(false);
        const closeOnKey = (e: KeyboardEvent) => {
            if (e.key === 'Escape') setOpen(false);
        };
        window.addEventListener('mousedown', closeOnOutside);
        window.addEventListener('scroll', closeOnScroll, true);
        window.addEventListener('keydown', closeOnKey);
        return () => {
            window.removeEventListener('mousedown', closeOnOutside);
            window.removeEventListener('scroll', closeOnScroll, true);
            window.removeEventListener('keydown', closeOnKey);
        };
    }, [open]);

    useLayoutEffect(() => {
        if (!open || !tooltipRef.current) return;
        const rect = tooltipRef.current.getBoundingClientRect();
        const margin = 8;
        const overflowBottom = rect.bottom - (window.innerHeight - margin);
        if (overflowBottom <= 0) return;
        const top = Math.max(margin, position.top - overflowBottom);
        setPosition((prev) => (prev.top === top ? prev : { ...prev, top }));
    }, [open, position.top]);

    const toggleTooltip = () => {
        if (open) {
            setOpen(false);
            return;
        }
        const rect = triggerRef.current?.getBoundingClientRect();
        if (!rect) return;
        const contentWidth = Math.min(320, window.innerWidth - 16);
        const left = Math.min(
            Math.max(rect.left + rect.width / 2 - contentWidth / 2, 8),
            Math.max(8, window.innerWidth - contentWidth - 8));
        setPosition({ top: rect.bottom + 6, left });
        setOpen(true);
    };

    const renderDescription = () =>
        content.description.map((line, i) => <p key={i}>{line}</p>);

    const trigger = React.isValidElement(children)
        ? React.cloneElement(children as React.ReactElement<{ onClick?: () => void }>, { onClick: toggleTooltip })
        : children;

    return (
        <span ref={triggerRef} className="inline-flex">
            {trigger}
            {open && createPortal(
                <div
                    ref={tooltipRef}
                    className="fixed z-50 w-80 max-w-[calc(100vw-2rem)] rounded-lg bg-dark border border-bright/20 p-3 shadow-2xl"
                    style={{ top: position.top, left: position.left }}
                >
                    <div className="flex items-start justify-between gap-2">
                        <h3 className="text-sm font-semibold text-bright">{content.name}</h3>
                        <button
                            onClick={() => setOpen(false)}
                            className="flex-shrink-0 p-0.5 rounded text-muted hover:text-bright hover:bg-bright/10 transition-colors"
                            aria-label="Закрыть"
                        >
                            <X className="w-4 h-4" />
                        </button>
                    </div>
                    {content.imageName && (
                        <img
                            src={`/images/pictures/${content.imageName}.jpg`}
                            alt={content.name}
                            className="w-full mt-2 rounded-md object-cover"
                        />
                    )}
                    <div className="mt-2 text-xs text-light leading-relaxed space-y-1">
                        {renderDescription()}
                    </div>
                </div>,
                document.body
            )}
        </span>
    );
};

export default InfoTooltip;
