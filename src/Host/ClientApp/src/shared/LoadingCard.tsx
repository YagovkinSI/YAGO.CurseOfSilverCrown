import { CircularProgress } from '@mui/material';
import ModalCard from './ModalCard';

export const LoadingCard: React.FC = () => {
    return (
        <ModalCard
            severity={'info'}
            title={'Загрузка...'}
            text={'Пожалуйста, подождите...'}
            icon={<CircularProgress size={24} />}
        />
    );
};

export default LoadingCard