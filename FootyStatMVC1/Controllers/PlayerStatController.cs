using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Actions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Controllers.ConstraintViewModels;

namespace FootyStatMVC1.Controllers
{
    public class PlayerStatController : Controller
    {

        

        public class PlayerStatViewModel
        {
            // Constructors
            //public PlayerStatViewModel(List<string> idx_season)
            //{
            //    this.seasonIndex = idx_season;
            //    // Initialise the teamName index to empty
            //    this.teamNameIndex = new List<string>(10);
            //    // Same for playerName
            //    this.playerNameIndex = new List<string>(10);
            //}
            
            //public PlayerStatViewModel(List<string> idx_season, List<string> idx_teamName)
            //{
            //    this.seasonIndex = idx_season;
            //    this.teamNameIndex = idx_teamName;
            //    this.playerNameIndex = new List<string>(10);
            //}

            //public PlayerStatViewModel(List<string> idx_season, List<string> idx_teamName, List<string> idx_playerName, double pg)
            //{
            //    this.seasonIndex = idx_season;
            //    this.teamNameIndex = idx_teamName;
            //    this.playerNameIndex = idx_playerName;

            //    this.player_goals = pg;
            //}

            public PlayerStatViewModel(List<string> idx_season, 
                                        List<string> idx_teamName = null, 
                                        List<string> idx_playerName = null, 
                                        
                                        string sel_season = null, 
                                        string sel_teamName = null, 
                                        string sel_playerName = null,
                                        CurrentPlayerStatsViewModel cpsVM = null
                                        )
            {
                this.seasonIndex = idx_season;
                this.teamNameIndex = idx_teamName;
                this.playerNameIndex = idx_playerName;

                // indicies cannot be null, so initialise them if they are
                if (seasonIndex == null) seasonIndex = new List<string>();
                if (teamNameIndex == null) teamNameIndex = new List<string>();
                if (playerNameIndex == null) playerNameIndex = new List<string>();


                

                this.selected_seasonIndex = sel_season;
                this.selected_teamNameIndex = sel_teamName;
                this.selected_playerNameIndex = sel_playerName;

                // Set the stats view model
                statsVM = cpsVM;

                // Defaults to inactive constraints
                haCVM = new HomeAwayCVM();

                gwCVM = new GameweekCVM();

                IsHome = false;

            }



            // Default constructor
            public PlayerStatViewModel()
            {

            }

            





            // Constraint section (must break this out into another class)



            public HomeAwayCVM haCVM { get; set; }
            
            public GameweekCVM gwCVM { get; set; }

            public MinsPlayedCVM mpCVM { get; set; }

            public bool IsHome { get; set; }

            
            // Player stats view model
            public CurrentPlayerStatsViewModel statsVM {get; set;}



            // Season in?dex
            [DisplayName("Season")]
            public string selected_seasonIndex { get; set; }
            public List<string> seasonIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> seasonIndex_select_list
            {
                get 
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return seasonIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }

            // teamNameIndex
            [DisplayName("Team Name")]
            public string selected_teamNameIndex { get; set; }
            public List<string> teamNameIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> teamNameIndex_select_list
            {
                get
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return teamNameIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }


            // playerNameIndex
            [DisplayName("Player Name")]
            public string selected_playerNameIndex { get; set; }
            public List<string> playerNameIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> playerNameIndex_select_list
            {
                get
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return playerNameIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }


        }// PlayerStatViewModel
        
        
        











        
        //
        // GET: /PlayerStat/

        public SnapViewDirector get_svd()
        {
            return FootyStatMVC1.FootyStatInit.get_svd();
        }

        public ActionResult Index()
        {
            return View();
        }


        double get_total_player_goals()
        {
            return get_total_player_number_stat("goalScdPlayer");
        }

        double get_total_player_assists()
        {
            return get_total_player_number_stat("assistsPlayer");
        }


        // Iterate over the view and get the goals.
        double get_total_player_number_stat(string stat_field_name)
        {
            
            // First look for a TotallingAction in the SVD using the stat_field_name field. If it doesn't exist - remake it.
            // NEED TO THINK CAREFULLY AGAIN HERE: SHOULD THIS BE VALID/INVALID at some point?
            SnapViewDirector svd = FootyStatMVC1.FootyStatInit.get_svd();
            TotallingAction ta = svd.pullOutTotallingAction(stat_field_name);
            if (ta != null)
            {
                return ta.total;
            }
            // Else have to redo the calculation
            else
            {
                TotallingMC tmc = make_TA(stat_field_name);

                get_svd().AddRegistrationCandidate(tmc);
                get_svd().RegisterCandidates();

                


                // NOT de-registering this ... so should hang around
                // NEED TO THINK CAREFULLY ABOUT THIS - WHEN TO DE-REGISTER
                //tmc.remove_me();

                TotallingAction ta2 = (TotallingAction)tmc.get_action();
                


                return ta2.total;
            }
        }

        void filter_on_playerName(string playerName_choice)
        {
            string filter_field_name = "playerSurname";

            // Register a filter on the season column picking out the selected season
            FilterMC fmc = make_filter(filter_field_name, playerName_choice);

            get_svd().AddRegistrationCandidate(fmc);
            get_svd().RegisterCandidates();

            

            // Don't need this filter or index anymore, so remove it.
            get_svd().Detach(fmc);
            
        }


        // Feels like yet another helper which should live somewhere else...
        List<string> getplayerNameIndex(string season_choice)
        {
            string idx_field_name = "playerSurname";
            string filter_field_name = "teamName";
            List<string> rtn_idx = null;

            if (!get_field(idx_field_name).check_and_get_index(ref rtn_idx))
            {

                // This is all getting the playerName index...
                IndexMC tia = make_index(idx_field_name);
                get_svd().AddRegistrationCandidate(tia);

                // Register a filter on the season column picking out the selected season
                FilterMC fmc = make_filter(filter_field_name, season_choice);
                get_svd().AddRegistrationCandidate(fmc);
               
                
                
                get_svd().RegisterCandidates();

                

                // Don't need this filter or index anymore, so remove it.
                
                get_svd().Detach(fmc);
                get_svd().Detach(tia);

                rtn_idx = get_idx_str(tia);

                // cache in the field
                cache_index(idx_field_name, (IndexingAction)tia.get_action());

            }//if



            return rtn_idx;

        }






        // Feels like yet another helper which should live somewhere else...
        List<string> getTeamNameIndex(string season_choice)
        {
            string idx_field_name = "teamName";
            string filter_field_name = "season";
            List<string> rtn_idx = null;

            if (!get_field(idx_field_name).check_and_get_index(ref rtn_idx))
            {

                // This is all getting the teamName index...
                IndexMC tia = make_index(idx_field_name);
                get_svd().AddRegistrationCandidate(tia);


                // Register a filter on the season column picking out the selected season
                FilterMC fmc = make_filter(filter_field_name, season_choice);
                get_svd().AddRegistrationCandidate(fmc);

                get_svd().RegisterCandidates();

                

                // Don't need this filter or index anymore, so remove it.
               
                get_svd().Detach(fmc);
                get_svd().Detach(tia);

                rtn_idx = get_idx_str(tia);

                // cache in the field
                cache_index(idx_field_name, (IndexingAction)tia.get_action());

            }//if



            return rtn_idx;

        }

        void cache_index(string field_name, IndexingAction idx)
        {
            Field f = get_field(field_name);
            f.set_index(idx);
        }


        // Helper to get Field
        Field get_field(string field_name)
        {
            return FootyStatInit.get_snapview().findInDict(field_name);
        }


        // Helper to get the season index
        // Should this live somewhere else too????
        List<string> getSeasonIndex()
        {
            string idx_field_name = "season";
            List<string> rtn_idx = null;
            
            // Check the field for a cached version first
            // If there is a cached version, this line puts it in rtn_idx
            if (!get_field(idx_field_name).check_and_get_index(ref rtn_idx))
            {

                IndexMC imc = make_index(idx_field_name);
                get_svd().AddRegistrationCandidate(imc);

                get_svd().RegisterCandidates();

                // Remove this to be tidy.
                
                get_svd().Detach(imc);

                rtn_idx = get_idx_str(imc);

                // cache in the field
                cache_index(idx_field_name, (IndexingAction)imc.get_action());

            }
            
            return rtn_idx;

        }// getSeasonIndex

        // Helper to pull out the List<string> of index values
        List<string> get_idx_str(IndexMC imc)
        {
            IndexingAction ia = (IndexingAction)imc.get_action();
            return ia.getStrLst();
        }

        // Helper to register a new index action with the SVD
        // This should also live somewhere else....
        IndexMC make_index(string field_name)
        {
            Field f = get_field(field_name);
            IndexMC imc = new IndexMC(FootyStatMVC1.FootyStatInit.get_svd(), new IndexingAction(f));
            return imc;
        }

        // Helper to register a filter:
        //   - field_name is which field we are filtering
        //   - choice is the particular value of this column we are selecting
        FilterMC make_filter(string field_name, string choice)
        {
            Field f = get_field(field_name);
            StringEqualsFilter season_filter = new StringEqualsFilter(field_name, f, choice);
            // Create new filtering colleague, and register with director
            FilterMC fmc = new FilterMC(FootyStatMVC1.FootyStatInit.get_svd(), season_filter);
            
            
            return fmc;
        }


        TotallingMC make_TA(string field_name)
        {
            return new TotallingMC(
                                   FootyStatMVC1.FootyStatInit.get_svd(), 
                                   new TotallingAction(get_field(field_name))
                                   );
        }



        string get_fieldChoice(string field_name)
        {
            Field f = get_field(field_name);
            if (f.projectedOut) return f.projectedVal;
            else return null;
        }

        string get_seasonChoice()
        {
            return get_fieldChoice("season");
        }

        string get_teamNameChoice()
        {
            return get_fieldChoice("teamName");
        }

        string get_playerNameChoice()
        {
            return get_fieldChoice("playerSurname");
        }


        CurrentPlayerStatsViewModel get_statsVM()
        {
            double goals = get_total_player_goals();
            double assists = get_total_player_assists();

            // Make the conversion to integer as we go into the view
            CurrentPlayerStatsViewModel statsVM = new CurrentPlayerStatsViewModel(Convert.ToInt16(goals), Convert.ToInt16(assists));

            return statsVM;
        
        }

        // ***************** END HELPERS
















        // Get version
        public ActionResult SelectPlayer()
        {
            List<string> idx_list = getSeasonIndex();
            return View(new PlayerStatViewModel(idx_list));
        }

        // Default Post version for rendering the html given the new viewModel
        [HttpPost]
        public ActionResult SelectPlayer(PlayerStatViewModel viewModel)
        {
            // This shouldn't fire with the current setup as submits are routed to other action methods.

            return View(viewModel);

        }


        // Test area for cleaner multiple submit framework

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenSeason")]
        public ActionResult ChosenSeason(PlayerStatViewModel viewModel) 
        {
            // This no longer valid way of handling this validation as need to redirect to PlayerStat
            // [SERVER SIDE VALIDATION SHOULD BE HANDLED HERE]
            //if (!ModelState.IsValid)
            //    return View(viewModel);

            List<string> team_idx = getTeamNameIndex(viewModel.selected_seasonIndex);

            return View("SelectPlayer", new PlayerStatViewModel(new List<string>(), team_idx, new List<string>(), viewModel.selected_seasonIndex, null, null));

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenTeam")]
        public ActionResult ChosenTeam(PlayerStatViewModel viewModel)
        {

            //if (!ModelState.IsValid)
            //    return View(viewModel);

            List<string> player_idx = getplayerNameIndex(viewModel.selected_teamNameIndex);

            return View("SelectPlayer", new PlayerStatViewModel(new List<string>(), new List<string>(), player_idx, get_seasonChoice(), viewModel.selected_teamNameIndex, null));

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenPlayer")]
        public ActionResult ChosenPlayer(PlayerStatViewModel viewModel)
        {

            //if (!ModelState.IsValid)
            //    return View(viewModel);

            filter_on_playerName(viewModel.selected_playerNameIndex);

            double player_goals = get_total_player_goals();

            return View("SelectPlayer", new PlayerStatViewModel(
                                                                new List<string>(), 
                                                                new List<string>(), 
                                                                new List<string>(), 
                                                                
                                                                get_seasonChoice(), 
                                                                get_teamNameChoice(), 
                                                                viewModel.selected_playerNameIndex,
                                                                get_statsVM()
                                                                ));

        }
        

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ApplyConstraints")]
        public ActionResult ApplyConstraints(PlayerStatViewModel viewModel)
        {

            // Constraints have changed - this should either automatically trigger an iteration - 
            // or at the very least set the chain reaction of through the SVD setting valid flag to false in snapview.

            
            // Debug: pull out TotallingAction (should be only 1)
            string stat_field_name = "goalScdPlayer";
            SnapViewDirector svd = FootyStatMVC1.FootyStatInit.get_svd();
            TotallingAction ta = svd.pullOutTotallingAction(stat_field_name);

            

            

            // Register constraints
            if (viewModel.haCVM.active)
            {
                HomeAway ha_enum;
                if(viewModel.haCVM.IsHome)ha_enum = HomeAway.Home;
                else ha_enum = HomeAway.Away;
                
                ConstraintMC cmc = new ConstraintMC(FootyStatMVC1.FootyStatInit.get_svd(), new HomeAwayConstraint(get_field("homeAway"), ha_enum));
                get_svd().AddRegistrationCandidate(cmc);
                

            }

            if (viewModel.gwCVM.active)
            {

                ConstraintMC cmc = new ConstraintMC(FootyStatMVC1.FootyStatInit.get_svd(), 
                                          new GameweekConstraint(get_field("Gameweek"), 
                                                                 viewModel.gwCVM.min.ToString(),
                                                                 viewModel.gwCVM.max.ToString()
                                                                 )
                                          );
                get_svd().AddRegistrationCandidate(cmc);
                
            }

            if (viewModel.mpCVM.active)
            {
                ConstraintMC cmc = new ConstraintMC(FootyStatMVC1.FootyStatInit.get_svd(), 
                                          new MinsPlayedConstraint(get_field("minsPlayed"),
                                                                   viewModel.mpCVM.val.ToString()
                                                                   )
                                          );
                get_svd().AddRegistrationCandidate(cmc);
                
            }

            // Register all the candidate MC's we have accumulated (which also triggers the snapview iterate)
            get_svd().RegisterCandidates();


            // De-register the constraints so the next submission of the constraints is clean
            get_svd().remove_all_ConstraintMC();

            
            // Update the stat
            double player_goals = get_total_player_goals();

            // Meaningless comment

            return View("SelectPlayer", new PlayerStatViewModel(
                                        new List<string>(), 
                                        new List<string>(), 
                                        new List<string>(), 
                                        
                                        get_seasonChoice(), 
                                        get_teamNameChoice(), 
                                        get_playerNameChoice(),
                                        get_statsVM()
                                        ));

        }





        public ActionResult testMethod()
        {

            return View();
        }

    }
}
