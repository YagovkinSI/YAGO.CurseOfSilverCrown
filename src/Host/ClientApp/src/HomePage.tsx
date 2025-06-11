import YagoCard from './shared/YagoCard';
import YagoLogo from './features/YagoLogo';

const HomePage: React.FC = () => {

  return (
    <YagoCard title={'Yago World'} >
      <div>
        <YagoLogo />
      </div>
    </YagoCard>
  )
}

export default HomePage