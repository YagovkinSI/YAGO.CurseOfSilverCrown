import { Alert, AlertTitle, Fade } from '@mui/material';

export interface ModalCardProps {
    severity: "info" | "error",
    title: string,
    text: string,
    icon?: React.ReactNode | undefined
    backgroundColor?: string | undefined
}

const ModalCard: React.FC<ModalCardProps> = ({ severity, title, text, icon, backgroundColor = 'var(--color-light)' }) => {
    return (
        <Fade in timeout={500}>
            <Alert
                severity={severity}
                icon={icon}
                sx={{
                    mt: '1rem',
                    margin: '1rem',
                    position: 'fixed',
                    bottom: '40px',
                    left: '10px',
                    zIndex: 'var(--z-index-modal)',
                    backgroundColor: { backgroundColor },
                    border: '1px solid',
                    borderColor: 'divider',
                    boxShadow: 3,
                    width: 'auto'
                }}
            >
                <AlertTitle>{title}</AlertTitle>
                {text}
            </Alert>
        </Fade>
    );
};

export default ModalCard