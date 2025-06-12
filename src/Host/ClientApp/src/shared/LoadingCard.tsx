import { Alert, AlertTitle, CircularProgress, Fade } from '@mui/material';

export const LoadingCard: React.FC = () => {
    return (
        <Fade in timeout={500}>
            <Alert
                severity="info"
                icon={<CircularProgress size={24} />}
                sx={{
                    mt: '1rem',
                    margin: '1rem',
                    position: 'fixed',
                    bottom: '40px',
                    left: '10px',
                    zIndex: 'var(--z-index-modal)',
                    backgroundColor: 'background.paper',
                    border: '1px solid',
                    borderColor: 'divider',
                    boxShadow: 3,
                    width: 'auto'
                }}
            >
                <AlertTitle>
                    Загрузка...
                </AlertTitle>
                Пожалуйста, подождите...
            </Alert>
        </Fade>
    );
};

export default LoadingCard